using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace BasicCRM.Business.Dtos.ClientDto
{
    public record GetClientDto
    {
        public Guid ClientID { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public Guid AddressID { get; set; }
        public GetAddressDto? Address { get; init; }
    }
}
