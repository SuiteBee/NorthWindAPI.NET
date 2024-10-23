using AutoMapper;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.ResponseDto;
using NorthWindAPI.Services.Interfaces;
using NorthWindAPI.Controllers.Models.Requests;

namespace NorthWindAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerRepository _customerRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        private readonly string dateFormat = "yyyy-MM-dd";

        public OrderService(IOrderRepository orderRepository, IEmployeeRepository employeeRepository, ICustomerRepository customerRepository, IMapper mapper, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> ListOrders()
        {
            var orderList = await _orderRepository.AllOrders();
            var detailList = await _orderRepository.AllDetails();
            var productList = await _orderRepository.AllProducts();
            var categoryList = await _orderRepository.AllCategories();
            var employeeList = await _employeeRepository.AllEmployees();
            var carrierList = await _orderRepository.AllCarriers();
            var customerList = await _customerRepository.AllCustomers();

            var orderDto = _mapper.Map<IEnumerable<OrderDto>>(orderList);

            foreach(OrderDto dto in orderDto)
            {
                var order = orderList.First(x => x.Id == dto.OrderId);
                _mapper.Map(order, dto.SendTo);
                _mapper.Map(order, dto.SendTo.Address);

                var detailsDto = detailList.Where(x => x.OrderId == dto.OrderId);
                foreach(OrderDetail detail in detailsDto)
                {
                    Product prod = productList.First(x => x.Id == detail.ProductId);
                    Category cat = categoryList.First(x => x.Id == prod.CategoryId);

                    ProductDto prodDto = _mapper.Map<ProductDto>(prod);
                    _mapper.Map(cat, prodDto);
                    _mapper.Map(detail, prodDto);

                    CalculateProductTotals(prodDto);

                    dto.Products.Add(prodDto);
                }

                var completedBy = employeeList.First(x => x.Id == order.EmployeeId);
                _mapper.Map(completedBy, dto.CompletedBy);

                var shippedBy = carrierList.First(x => x.Id == order.ShipVia);
                _mapper.Map(shippedBy, dto.SendTo);

                var orderedBy = customerList.First(x => x.Id == order.CustomerId);
                _mapper.Map(orderedBy, dto.OrderedBy);
                _mapper.Map(orderedBy, dto.OrderedBy.ContactInfo);

                CalculateOrderTotal(dto);
            }

            return orderDto;
        }

        public async Task<OrderDto> FindOrder(int id)
        {
            var orderBase = await _orderRepository.FindOrder(id);
            var orderDetails = await _orderRepository.FindDetail(id);

            var orderDto = _mapper.Map<OrderDto>(orderBase);
            _mapper.Map(orderBase, orderDto.SendTo);
            _mapper.Map(orderBase, orderDto.SendTo.Address);

            foreach (OrderDetail detail in orderDetails)
            {
                Product prod = await _orderRepository.FindProduct(detail.ProductId);
                Category cat = await _orderRepository.FindCategory(prod.CategoryId);

                ProductDto prodDto = _mapper.Map<ProductDto>(prod);
                _mapper.Map(cat, prodDto);
                _mapper.Map(detail, prodDto);

                CalculateProductTotals(prodDto);

                orderDto.Products.Add(prodDto);
            };

            var completedBy = await _employeeRepository.FindEmployee(orderBase.EmployeeId);
            _mapper.Map(completedBy, orderDto.CompletedBy);

            var shippedBy = await _orderRepository.FindCarrier(orderBase.ShipVia);
            _mapper.Map(shippedBy, orderDto.SendTo);

            var orderedBy = await _customerRepository.FindCustomer(orderBase.CustomerId);
            _mapper.Map(orderedBy, orderDto.OrderedBy);
            _mapper.Map(orderedBy, orderDto.OrderedBy.ContactInfo);

            CalculateOrderTotal(orderDto);

            return orderDto;

        }

        public async Task<OrderDto> ProcessNewOrder(NewOrderRequest newOrder)
        {
            var toInsert = _mapper.Map<Order>(newOrder);
            _mapper.Map(newOrder.Address, toInsert);

            var orderDate = DateTime.Today;
            var orderDateString = orderDate.ToString(dateFormat);
            toInsert.OrderDate = orderDateString;
            toInsert.RequiredDate = orderDate.AddDays(28).ToString(dateFormat);

            var inserted = await _orderRepository.InsertOrder(toInsert);

            foreach(var detail in newOrder.OrderDetail)
            {
                Product prod = await _orderRepository.FindProduct(detail.ProductId);

                var toInsertDetail = _mapper.Map<OrderDetail>(detail);
                toInsertDetail.Id = $"{inserted.Id}/{detail.ProductId}";
                toInsertDetail.OrderId = inserted.Id;
                toInsertDetail.UnitPrice = prod.UnitPrice;

                await _orderRepository.InsertDetail(toInsertDetail);
            }

            return await FindOrder(inserted.Id);
        }

        #region " Business Logic "

        private void CalculateOrderTotal(OrderDto dto)
        {
            var prodTotal = dto.Products.Sum(x => x.FinalPrice);
            dto.OrderTotal = prodTotal + dto.SendTo.ShipCost;
        }

        private void CalculateProductTotals(ProductDto dto)
        {
            dto.TotalPrice = GetTotalPrice(dto);
            dto.FinalPrice = GetFinalPrice(dto);
        }

        private decimal GetTotalPrice(ProductDto dto)
        {
            return Math.Round(dto.PurchasePrice * dto.Quantity, 2);
        }

        private decimal GetFinalPrice(ProductDto dto)
        {
            if(dto.Discount > 0)
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
