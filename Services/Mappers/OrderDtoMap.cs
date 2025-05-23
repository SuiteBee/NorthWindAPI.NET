﻿using AutoMapper;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Mappers
{
    public class OrderDtoMap : Profile
    {
        public OrderDtoMap()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id));

            CreateMap<Order, ShippingDto>()
                .ForMember(d => d.ShipCost, o => o.MapFrom(s => s.Freight));

            CreateMap<Shipper, ShippingDto>()
                .ForMember(d => d.CarrierId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ShipCarrier, o => o.MapFrom(s => s.CompanyName));

            CreateMap<Order, AddressDto>()
                .ForMember(d => d.Street, o => o.MapFrom(s => s.ShipAddress))
                .ForMember(d => d.City, o => o.MapFrom(s => s.ShipCity))
                .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.ShipPostalCode))
                .ForMember(d => d.Country, o => o.MapFrom(s => s.ShipCountry))
                .ForMember(d => d.Region, o => o.MapFrom(s => s.ShipRegion));


            CreateMap<Product, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ItemPrice, o => o.MapFrom(s => s.UnitPrice));

            CreateMap<Category, OrderItemDto>();

            CreateMap<OrderDetail, OrderItemDto>()
                .ForMember(d => d.PurchasePrice, o => o.MapFrom(s => s.UnitPrice));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<OrderDetail, OrderDto>();
            CreateMap<Product, OrderDto>();
            CreateMap<Customer, OrderDto>();
            CreateMap<Employee, OrderDto>();
            CreateMap<ShippingDto, OrderDto>();
        }
    }
}
