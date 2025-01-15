using AutoMapper;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
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

                    CalculateItemTotals(itemDto);

                    dto.Items.Add(itemDto);
                }

                var shippedBy = carrierList.First(x => x.Id == order.ShipVia);
                _mapper.Map(shippedBy, dto.SendTo);

                CalculateOrderTotal(dto);
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

                CalculateItemTotals(itemDto);

                orderDto.Items.Add(itemDto);
            };

            var shippedBy = await _orderRepository.FindCarrier(orderBase.ShipVia);
            _mapper.Map(shippedBy, orderDto.SendTo);

            CalculateOrderTotal(orderDto);

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
            var toInsert = _mapper.Map<Order>(newOrder);
            _mapper.Map(newOrder.Address, toInsert);

            var orderDate = DateTime.Today;
            var orderDateString = orderDate.ToString(dateFormat);
            toInsert.OrderDate = orderDateString;
            toInsert.RequiredDate = orderDate.AddDays(28).ToString(dateFormat);

            var inserted = await _orderRepository.InsertOrder(toInsert);

            foreach (var detail in newOrder.OrderDetail)
            {
                Product prod = await _productRepository.FindProduct(detail.ProductId);

                var toInsertDetail = _mapper.Map<OrderDetail>(detail);
                toInsertDetail.Id = $"{inserted.Id}/{detail.ProductId}";
                toInsertDetail.OrderId = inserted.Id;

                var itemDiscount = detail.Discount / 100;
                toInsertDetail.Discount = (double)itemDiscount;
                toInsertDetail.UnitPrice = prod.UnitPrice - Math.Round(prod.UnitPrice * itemDiscount, 2);

                await _orderRepository.InsertDetail(toInsertDetail);
            }

            return await FindOrder(inserted.Id);
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

            return toReturn;
        }

        #endregion

        #region " DELETE "

        public async Task<bool> RemoveOrder(int orderId)
        {
            return await _orderRepository.DeleteOrder(orderId);
        }

        #endregion

        #region " Business Logic "

        private static void CalculateOrderTotal(OrderDto dto)
        {
            dto.OrderSubtotal = dto.Items.Sum(x => x.FinalPrice);
            dto.OrderTotal = dto.OrderSubtotal + dto.SendTo.ShipCost;
        }

        private static void CalculateItemTotals(OrderItemDto dto)
        {
            dto.TotalPrice = GetTotalPrice(dto);
            dto.DiscountAmt = GetDiscount(dto);
            dto.FinalPrice = GetFinalPrice(dto);
        }

        private static decimal GetTotalPrice(OrderItemDto dto)
        {
            return Math.Round(dto.ItemPrice * dto.Quantity, 2);
        }

        private static decimal GetDiscount(OrderItemDto dto)
        {
            return dto.TotalPrice * (decimal)dto.Discount;
        }

        private static decimal GetFinalPrice(OrderItemDto dto)
        {
            if (dto.Discount > 0)
            {
                decimal finalPrice = dto.TotalPrice * (decimal)(1 - dto.Discount);
                return Math.Round(finalPrice, 2);
            }
            else
            {
                return dto.TotalPrice;
            }
        }

        #endregion
    }
}
