using AutoMapper;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Services.Mappers
{
    public class OrderRequestMap : Profile
    {
        public OrderRequestMap()
        {
            CreateMap<NewOrderRequest, Order>()
                .ForMember(d => d.ShipVia, o => o.MapFrom(s => s.CarrierId))
                .ForMember(d => d.Freight, o => o.MapFrom(s => s.ShipCost));

            CreateMap<AddressRequest, Order>()
                .ForMember(d => d.ShipAddress, o => o.MapFrom(s => s.Street))
                .ForMember(d => d.ShipCity, o => o.MapFrom(s => s.City))
                .ForMember(d => d.ShipRegion, o => o.MapFrom(s => s.Region))
                .ForMember(d => d.ShipPostalCode, o => o.MapFrom(s => s.PostalCode))
                .ForMember(d => d.ShipCountry, o => o.MapFrom(s => s.Country));

            CreateMap<OrderDetailRequest, OrderDetail>();

        }
    }
}
