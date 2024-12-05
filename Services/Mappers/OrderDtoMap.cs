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
                .ForMember(d => d.ShipCarrier, o => o.MapFrom(s => s.CompanyName));

            CreateMap<Order, AddressDto>()
                .ForMember(d => d.Street, o => o.MapFrom(s => s.ShipAddress))
                .ForMember(d => d.City, o => o.MapFrom(s => s.ShipCity))
                .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.ShipPostalCode))
                .ForMember(d => d.Country, o => o.MapFrom(s => s.ShipCountry))
                .ForMember(d => d.Region, o => o.MapFrom(s => s.ShipRegion));

            
            CreateMap<Product, OrderItemDto>();

            CreateMap<Category, OrderItemDto>();

            CreateMap<OrderDetail, OrderItemDto>()
                .ForMember(d => d.PurchasePrice, o => o.MapFrom(s => s.UnitPrice));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<Customer, CustomerDto>()
                .ForMember(d => d.ContactInfo, o => o.Ignore())
                .ForMember(d => d.Address, o => o.Ignore());

            CreateMap<Customer, ContactDto>();
            CreateMap<Customer, AddressDto>()
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City))
                .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.PostalCode))
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Country))
                .ForMember(d => d.Region, o => o.MapFrom(s => s.Region));

            CreateMap<OrderDetail, OrderDto>();
            CreateMap<Product, OrderDto>();
            CreateMap<Customer, OrderDto>();
            CreateMap<Employee, OrderDto>();
            CreateMap<ShippingDto, OrderDto>();
        }
    }
}
