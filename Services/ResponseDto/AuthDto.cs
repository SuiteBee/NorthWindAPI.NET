namespace NorthWindAPI.Services.ResponseDto
{
    public class AuthDto
    {
        public bool Authorized { get; set; }
        public required string UserName { get; set; }
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }
    }
}
