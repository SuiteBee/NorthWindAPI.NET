namespace NorthWindAPI.Controllers.Models.Requests
{
    public class AuthRequest
    {
        public required string Usr { get; set; }
        public required string Pwd { get; set; }
    }
}
