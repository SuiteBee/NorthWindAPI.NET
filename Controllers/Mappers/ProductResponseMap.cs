using AutoMapper;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Mappers
{
    public class ProductResponseMap : Profile
    {
        public ProductResponseMap()
        {
            CreateMap<ProductDto, ProductResponse>();
            CreateMap<SupplierDto, SupplierResponse>();
        }
    }
}
