using AutoMapper;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Mappers
{
    public class CustomerDtoMap : Profile
    {
        public CustomerDtoMap()
        {

            CreateMap<Customer, CustomerDto>()
                .ForMember(d => d.ContactInfo, o => o.Ignore())
                .ForMember(d => d.Address, o => o.Ignore());

            CreateMap<Customer, ContactDto>()
                .AddTransform<string>(s => string.IsNullOrEmpty(s) ? "" : s);

            CreateMap<Customer, AddressDto>()
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City))
                .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.PostalCode))
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Country))
                .ForMember(d => d.Region, o => o.MapFrom(s => s.Region));

            CreateMap<CustomerDto, Customer>()
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address.Street))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
                .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.Address.PostalCode))
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Address.Country))
                .ForMember(d => d.Region, o => o.MapFrom(s => s.Address.Region))
                .ForMember(d => d.ContactName, o => o.MapFrom(s => s.ContactInfo.ContactName))
                .ForMember(d => d.ContactTitle, o => o.MapFrom(s => s.ContactInfo.ContactTitle))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.ContactInfo.Phone))
                .ForMember(d => d.Fax, o => o.MapFrom(s => s.ContactInfo.Fax));
        }
    }
}
