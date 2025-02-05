using AutoMapper;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Mappers
{
    public class OrderResponseMap : Profile
    {
        public OrderResponseMap()
        {
            CreateMap<OrderDto, OrderResponse>()
               .ForMember(d => d.OrderedBy, o => o.MapFrom(s => new CustomerResponse { Id = s.CustomerId }))
               .ForMember(d => d.CompletedBy, o => o.MapFrom(s => new EmployeeResponse { Id = s.EmployeeId }))
               .ForMember(d => d.Fulfilled, o => o.MapFrom(s => !string.IsNullOrEmpty(s.SendTo.ShippedDate)));
        }
    }
}
