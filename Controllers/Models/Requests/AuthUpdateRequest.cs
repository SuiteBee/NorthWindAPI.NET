namespace NorthWindAPI.Controllers.Models.Requests
{
    public class AuthUpdateRequest
    {
        public required string usr { get; set; }
        public required string pwd { get; set; }
        public required string newPwd { get; set; }
    }
}
