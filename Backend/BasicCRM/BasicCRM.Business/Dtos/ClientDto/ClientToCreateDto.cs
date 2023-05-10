using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Dtos.ClientDto
{
    public record ClientToCreateDto
    {
        public string FirstName { get; init; } 
        public string LastName { get; init; } 
        public DateTime DateOfBirth { get; init; }
        public string Email { get; init; } 
        public string PhoneNumber { get; init; }
        public Guid AddressID { get; set; }
    }
}
