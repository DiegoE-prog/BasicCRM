using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicCRM.Business.Dtos.ClientDto
{
    public class GetClientDto : GetAddressDto
    {
        public Guid ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public DateTime DateOfBirth { get; set; }
        public string DateOfBirthday
        {
            get { return DateOfBirth.ToString("MM/dd/yyyy"); }
            set { }
        }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid AddressID { get; set; }
    }
}
