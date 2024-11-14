using AutoMapper;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Mappers
{
    public class OrderResponseMap : Profile
    {
        public OrderResponseMap()
        {
            CreateMap<OrderDto, OrderResponse>()
               .ForMember(d => d.OrderedBy, o => o.MapFrom(s => new CustomerDto { Id = s.CustomerId }))
               .ForMember(d => d.CompletedBy, o => o.MapFrom(s => new EmployeeDto { Id = s.EmployeeId }));           
        }
    }
}
