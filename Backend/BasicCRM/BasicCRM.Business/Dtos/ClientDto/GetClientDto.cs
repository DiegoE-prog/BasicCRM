using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Data.Entities;

namespace BasicCRM.Business.Dtos.ClientDto
{
    public record GetClientDto
    {
        public Guid ClientID { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; } 
        public DateTime DateOfBirth { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public GetAddressDto? Address { get; init; }
    }
}
