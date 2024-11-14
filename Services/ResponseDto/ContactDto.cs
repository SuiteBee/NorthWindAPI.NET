using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class ContactDto
    {
        public required string ContactName { get; set; }
        public required string ContactTitle { get; set; }
        public string CourtestyTitle { get; set; }
        public required string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }

        [SetsRequiredMembers]
        public ContactDto()
        {
            ContactName = "";
            ContactTitle = "";
            CourtestyTitle = "";
            Phone = "";
            Fax = "";
            Website = "";
        }
    }
}
