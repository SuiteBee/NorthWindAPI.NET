using AutoMapper;
using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Mappers
{
    public class CustomerRequestMap : Profile
    {
        public CustomerRequestMap()
        {
            CreateMap<CustomerRequest, CustomerDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.CompanyIdentifier));

            CreateMap<AddressRequest, AddressDto>();
            CreateMap<ContactRequest, ContactDto>();

            CreateMap<CustomerRequest, Customer>()
               .ForMember(d => d.Id, o => o.MapFrom(s => s.CompanyIdentifier));

            CreateMap<AddressRequest, Customer>()
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Street));

            CreateMap<ContactRequest, Customer>();
        }
    }
}
