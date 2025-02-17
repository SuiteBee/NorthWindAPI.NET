using AutoMapper;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.Calculations;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services
{
    public class OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMapper mapper, ILogger<OrderService> logger
    ) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductRepository _productRepository = productRepository;

        private readonly IMapper _mapper = mapper;
        private readonly ILogger<OrderService> _logger = logger;

        private readonly string dateFormat = "yyyy-MM-dd";

        #region " SELECT "

        public async Task<IEnumerable<OrderDto>> ListOrders()
        {
            var orderList = await _orderRepository.AllOrders();
            var detailList = await _orderRepository.AllDetails();
            var productList = await _productRepository.AllProducts();
            var categoryList = await _productRepository.AllCategories();
            var carrierList = await _orderRepository.AllCarriers();

            var orderDto = _mapper.Map<IEnumerable<OrderDto>>(orderList);

            foreach (OrderDto dto in orderDto)
            {
                var order = orderList.First(x => x.Id == dto.OrderId);
                _mapper.Map(order, dto.SendTo);
                _mapper.Map(order, dto.SendTo.Address);

                var detailsDto = detailList.Where(x => x.OrderId == dto.OrderId);
                foreach (OrderDetail detail in detailsDto)
                {
                    Product prod = productList.First(x => x.Id == detail.ProductId);
                    Category cat = categoryList.First(x => x.Id == prod.CategoryId);

                    OrderItemDto itemDto = _mapper.Map<OrderItemDto>(prod);
                    _mapper.Map(cat, itemDto);
                    _mapper.Map(detail, itemDto);

                    OrderCalculations.ItemTotals(itemDto);

                    dto.Items.Add(itemDto);
                }

                var shippedBy = carrierList.First(x => x.Id == order.ShipVia);
                _mapper.Map(shippedBy, dto.SendTo);

                OrderCalculations.OrderTotal(dto);
            }

            return orderDto;
        }

        public async Task<OrderDto> FindOrder(int orderId)
        {
            var orderBase = await _orderRepository.FindOrder(orderId);
            var orderDetails = await _orderRepository.FindDetail(orderId);

            if (orderBase == null)
            {
                throw new BadHttpRequestException("Resource does not exist");
            }

            var orderDto = _mapper.Map<OrderDto>(orderBase);
            _mapper.Map(orderBase, orderDto.SendTo);
            _mapper.Map(orderBase, orderDto.SendTo.Address);

            foreach (OrderDetail detail in orderDetails)
            {
                Product prod = await _productRepository.FindProduct(detail.ProductId);
                Category cat = await _productRepository.FindCategory(prod.CategoryId);

                OrderItemDto itemDto = _mapper.Map<OrderItemDto>(prod);
                _mapper.Map(cat, itemDto);
                _mapper.Map(detail, itemDto);

                OrderCalculations.ItemTotals(itemDto);

                orderDto.Items.Add(itemDto);
            };

            var shippedBy = await _orderRepository.FindCarrier(orderBase.ShipVia);
            _mapper.Map(shippedBy, orderDto.SendTo);

            OrderCalculations.OrderTotal(orderDto);

            return orderDto;

        }

        public async Task<IEnumerable<CarrierDto>> Carriers()
        {
            var carriers = await _orderRepository.AllCarriers();
            var shipOptions = carriers.Select(c => new CarrierDto { Id = c.Id, CompanyName = c.CompanyName, Phone = c.Phone });
            return shipOptions.ToList();
        }

        #endregion

        #region " INSERT "

        public async Task<OrderDto> ProcessNewOrder(NewOrderRequest newOrder)
        {
            var newOrderId = -1;

            //Write operations performed on multiple sources will require a transaction
            using (var transaction = _orderRepository.BeginTransaction())
            {
                //Map request order model to resource
                var toInsert = _mapper.Map<Order>(newOrder);
                _mapper.Map(newOrder.Address, toInsert);

                //Set dates based on when request is made
                var orderDate = DateTime.Today;
                var orderDateString = orderDate.ToString(dateFormat);
                toInsert.OrderDate = orderDateString;
                toInsert.RequiredDate = orderDate.AddDays(28).ToString(dateFormat);

                //Insert to order table and save
                //We will need the inserted ID for order details
                var inserted = await _orderRepository.InsertOrder(toInsert);
                await _orderRepository.Save();
                newOrderId = inserted.Id;

                //Insert detail records for each product referencing inserted OrderID
                var details = new List<OrderDetail>();
                foreach (var detail in newOrder.OrderDetail)
                {
                    Product prod = await _productRepository.FindProduct(detail.ProductId);

                    //Map request order detail model to resource
                    var toInsertDetail = _mapper.Map<OrderDetail>(detail);
                    toInsertDetail.Id = $"{inserted.Id}/{detail.ProductId}";
                    toInsertDetail.OrderId = inserted.Id;

                    //Calculate unit price based on system price instead of request
                    var itemDiscount = detail.Discount / 100;
                    toInsertDetail.Discount = (double)itemDiscount;
                    toInsertDetail.UnitPrice = prod.UnitPrice - Math.Round(prod.UnitPrice * itemDiscount, 2);

                    details.Add(toInsertDetail);
                }

                await _orderRepository.InsertDetails(details);

                //Update product stock - subtract quantity ordered from units in stock
                await RemoveStock(newOrder.OrderDetail);

                _orderRepository.CommitTransaction(transaction);
            }

            return await FindOrder(newOrderId);
        }

        #endregion

        #region " UPDATE "

        public async Task<OrderDto> MarkAsShipped(int orderId, string? shippedDate = null)
        {
            var orderBase = await _orderRepository.FindOrder(orderId);

            DateTime shipDate = shippedDate == null ? DateTime.Today : DateTime.Parse(shippedDate);
            var shipDateString = shipDate.ToString(dateFormat);
            orderBase.ShippedDate = shipDateString;

            await _orderRepository.UpdateOrder(orderId, orderBase);

            await _orderRepository.Save();

            return await FindOrder(orderId);
        }

        public async Task<IEnumerable<OrderDto>> MarkAsShipped(ShipRequest orders)
        {
            var toReturn = new List<OrderDto>();

            foreach (int order in orders.OrderIds)
            {
                var shipped = await MarkAsShipped(order, orders.ShipDate);
                toReturn.Add(shipped);
            }

            await _orderRepository.Save();

            return toReturn;
        }

        private async Task ReplaceStock(IEnumerable<OrderItemDto> orderItems)
        {
            try
            {
                var productsToUpdate = new List<Product>();

                //Update product stock - add quantity ordered from order to be deleted
                foreach (var item in orderItems)
                {
                    var prodBase = await _productRepository.FindProduct(item.ProductId);
                    prodBase.UnitsInStock += item.Quantity;
                    productsToUpdate.Add(prodBase);
                }

                await _productRepository.UpdateMultipleProducts(productsToUpdate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ": Order Service: Error re-stocking inventory.");
            }
        }

        private async Task RemoveStock(IEnumerable<OrderDetailRequest> orderDetails)
        {
            try
            {
                var productsToUpdate = new List<Product>();

                //Update product stock - subtract quantity ordered from units in stock
                foreach (var detail in orderDetails)
                {
                    var prodBase = await _productRepository.FindProduct(detail.ProductId);
                    prodBase.UnitsInStock -= detail.Quantity;
                    productsToUpdate.Add(prodBase);
                }

                await _productRepository.UpdateMultipleProducts(productsToUpdate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ": Order Service: Error removing inventory.");
            }
        }

        #endregion

        #region " DELETE "

        public async Task RemoveOrder(int orderId)
        {
            //Write operations performed on multiple sources will require a transaction
            using (var transaction = _orderRepository.BeginTransaction())
            {
                var order = await FindOrder(orderId);
                if (order == null)
                {
                    throw new Exception($"{orderId} does not exist.");
                }

                //Update product stock - add quantity ordered from order to be deleted
                await ReplaceStock(order.Items);

                //Remove pending details and order record
                await _orderRepository.DeleteOrder(orderId);

                 _orderRepository.CommitTransaction(transaction);
            }
        }

        #endregion

    }
}
