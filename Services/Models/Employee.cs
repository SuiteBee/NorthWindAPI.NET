using System.ComponentModel.DataAnnotations;

namespace NorthWindAPI.Services.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
