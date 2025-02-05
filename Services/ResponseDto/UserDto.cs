using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class UserDto
    {
        /// <summary>
        /// Unique employee identifier
        /// </summary>
        /// <example>1</example>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Unique employee username
        /// </summary>
        /// <example>tryans</example>
        public required string UserName { get; set; }
        /// <summary>
        /// Employee first name
        /// </summary>
        /// <example>Tim</example>
        public required string FirstName { get; set; }
        /// <summary>
        /// Employee last name
        /// </summary>
        /// <example>Ryans</example>
        public required string LastName { get; set; }
        /// <summary>
        /// Unique employee role identifier
        /// </summary>
        /// <example>1</example>
        public int RoleId { get; set; }
        /// <summary>
        /// Employee role name
        /// </summary>
        /// <example>Admin</example>
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
