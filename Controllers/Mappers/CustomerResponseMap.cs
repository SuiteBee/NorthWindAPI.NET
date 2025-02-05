using AutoMapper;
using AutoMapper.EquivalencyExpression;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Mappers
{
    public class CustomerResponseMap : Profile
    {
        public CustomerResponseMap()
        {
            CreateMap<CustomerDto, CustomerResponse>();
            CreateMap<AddressDto, AddressResponse>();
            CreateMap<ContactDto, ContactResponse>();

            CreateMap<CustomerDto, OrderResponse>()
                .ForMember(dst => dst.OrderId, opt => opt.Ignore())

                .EqualityComparison((src, dst) => src.Id == dst.OrderedBy.Id);
        }
    }
}
