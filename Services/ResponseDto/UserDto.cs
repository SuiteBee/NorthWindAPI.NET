using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class UserDto
    {
        public int EmployeeId { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int RoleId { get; set; }
        public required string RoleName { get; set; }

        [SetsRequiredMembers]
        public UserDto()
        {
            EmployeeId = -1;
            UserName = "";
            FirstName = "";
            LastName = "";
            RoleId = -1;
            RoleName = "";
        }
    }
}
