using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicCRM.Business.Dtos.ClientDto
{
    public record GetClientDto
    {
        public Guid ClientID { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        [JsonIgnore]
        public DateTime DateOfBirth { get; init; }
        public string DateOfBirthday
        {
            get { return DateOfBirth.ToString("MM/dd/yyyy"); }
            set { }
        }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public Guid AddressID { get; set; }
        public GetAddressDto? Address { get; set; }
    }
}
