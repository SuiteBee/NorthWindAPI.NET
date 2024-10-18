using AutoMapper;
using NorthWindAPI.Controllers;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.Dto;
using NorthWindAPI.Services.Interfaces;

namespace NorthWindAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerRepository _customerRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderService(IOrderRepository orderRepository, IEmployeeRepository employeeRepository, ICustomerRepository customerRepository, IMapper mapper, ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _employeeRepository = employeeRepository;
            _customerRepository = customerRepository;

            _mapper = mapper;
            _logger = logger;
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

                ProductDto prodDto = new ProductDto()
                {
                    ProductName = prod.ProductName,
                    CategoryName = cat.CategoryName,
                    ItemPrice = prod.UnitPrice,
                    Quantity = detail.Quantity,
                    FinalPrice = detail.UnitPrice,
                    Discount = detail.Discount
                };

                 orderDto.Products.Add(prodDto);
            };

            var completedBy = await _employeeRepository.FindEmployee(orderBase.EmployeeId);
            _mapper.Map(completedBy, orderDto.CompletedBy);

            var shippedBy = await _orderRepository.FindCarrier(orderBase.ShipVia);
            _mapper.Map(shippedBy, orderDto.SendTo);

            var orderedBy = await _customerRepository.FindCustomer(orderBase.CustomerId);
            _mapper.Map(orderedBy, orderDto.OrderedBy);
            _mapper.Map(orderedBy, orderDto.OrderedBy.ContactInfo);

            return orderDto;
        }
    }
}
