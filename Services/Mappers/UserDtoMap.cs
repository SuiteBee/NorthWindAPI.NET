using AutoMapper;
using NorthWindAPI.Data.Resources;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Mappers
{
    public class UserDtoMap : Profile
    {
        public UserDtoMap()
        {

            CreateMap<Employee, UserDto>()
                .ForMember(d => d.EmployeeId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.UserName, o => o.Ignore())
                .ForMember(d => d.RoleId, o => o.Ignore())
                .ForMember(d => d.RoleName, o => o.Ignore());


            CreateMap<Role, UserDto>()
                .ForMember(d => d.RoleId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.UserName, o => o.Ignore())
                .ForMember(d => d.EmployeeId, o => o.Ignore())
                .ForMember(d => d.FirstName, o => o.Ignore())
                .ForMember(d => d.LastName, o => o.Ignore());
        }
    }
}
