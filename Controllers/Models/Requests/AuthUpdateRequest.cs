namespace NorthWindAPI.Controllers.Models.Requests
{
    public class AuthUpdateRequest
    {
        public required string Usr { get; set; }
        public required string Pwd { get; set; }
        public required string NewPwd { get; set; }
    }
}
