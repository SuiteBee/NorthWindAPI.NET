using AutoMapper;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Mappers
{
    public class ProductDtoMap : Profile
    {
        public ProductDtoMap()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ItemPrice, o => o.MapFrom(s => s.UnitPrice));
            CreateMap<Category, ProductDto>()
                .ForMember(d => d.CategoryDescription, o => o.MapFrom(s => s.Description));

            CreateMap<Supplier, SupplierDto>()
                .ForMember(d => d.Company, o => o.MapFrom(s => s.CompanyName))
                .ForMember(d => d.Address, o => o.Ignore());

            CreateMap<Supplier, ContactDto>();

            CreateMap<Supplier, AddressDto>()
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address));
        }
    }
}
