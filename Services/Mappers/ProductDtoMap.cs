using AutoMapper;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.ResponseDto;
using System.Text.RegularExpressions;

namespace NorthWindAPI.Services.Mappers
{
    public class ProductDtoMap : Profile
    {
        public ProductDtoMap()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ItemPrice, o => o.MapFrom(s => s.UnitPrice));
            CreateMap<Category, ProductDto>()
                .ForMember(d => d.CategoryDescription, o => o.MapFrom(s => s.Description));

            CreateMap<Supplier, SupplierDto>()
                .ForMember(d => d.Company, o => o.MapFrom(s => s.CompanyName))
                .ForMember(d => d.Address, o => o.Ignore());

            CreateMap<Supplier, ContactDto>()
                .ForMember(d => d.Website, o => o.MapFrom(s => ExtractWebsite(s.Homepage)));

            CreateMap<Supplier, AddressDto>()
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address));
        }

        private string ExtractWebsite(string? homepage)
        {
            if (homepage != null && homepage.Contains('#'))
            {
                Regex extracted = new Regex(@"#(.*?)#");
                return extracted.Match(homepage).Groups[1].Value;
            }

            return "";
        }
    }
}
