using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        [SetsRequiredMembers]
        public EmployeeDto()
        {
            Id = -1;
            FirstName = "";
            LastName = "";
        }
    }
}
