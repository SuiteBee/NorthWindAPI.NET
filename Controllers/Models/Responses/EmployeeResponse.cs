namespace NorthWindAPI.Controllers.Models.Responses
{
    public class EmployeeResponse
    {
        /// <summary>
        /// Unique Employee Identifier
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        /// <summary>
        /// Employee First Name
        /// </summary>
        /// <example>Tim</example>
        public string FirstName { get; set; }
        /// <summary>
        /// Employee Last Name
        /// </summary>
        /// <example>Ryans</example>
        public string LastName { get; set; }
    }
}
