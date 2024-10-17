using AutoMapper;
using NorthWindAPI.Controllers;
using NorthWindAPI.Data.RepositoryInterfaces;
using NorthWindAPI.Services.Dto;
using NorthWindAPI.Services.Interfaces;

namespace NorthWindAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OrderDto> FindOrder(int id)
        {
            var orderBase = await _orderRepository.FindOrder(id);
            var orderDetail = await _orderRepository.FindDetail(id);

            var orderDto = _mapper.Map<OrderDto>(orderBase);
            _mapper.Map(orderDetail, orderDto);

            return orderDto;
        }
    }
}
