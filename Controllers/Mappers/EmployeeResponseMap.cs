using AutoMapper;
using NorthWindAPI.Controllers.Models.Responses;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Mappers
{
    public class EmployeeResponseMap : Profile
    {
        public EmployeeResponseMap()
        {
            CreateMap<EmployeeDto, EmployeeResponse>();
        }
    }
}
