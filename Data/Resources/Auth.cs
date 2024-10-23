namespace NorthWindAPI.Data.Resources
{
    public class Auth : Entity
    {
        public int RoleId { get; set; }
        public int EmployeeId { get; set; }
        public string? Username { get; set; }
        public string? Hash { get; set; }
    }
}
