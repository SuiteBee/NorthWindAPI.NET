using AutoMapper;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.Dto;

namespace NorthWindAPI.Services.Mappers
{
    public class MappingOrder : Profile
    {
        public MappingOrder()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDto>();
            CreateMap<Product, OrderDto>();
            CreateMap<Customer, OrderDto>();
            CreateMap<Employee, OrderDto>();
            CreateMap<ShippingDto, OrderDto>();
        }
    }
}
