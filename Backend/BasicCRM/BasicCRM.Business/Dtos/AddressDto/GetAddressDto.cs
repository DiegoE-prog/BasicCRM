using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Dtos.AddressDto
{
    public record GetAddressDto
    {
        public Guid AddressID { get; init; }
        public string AddressLine { get; init; }
        public string? AddressDetails { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public int ZipCode { get; init; }
        public string Country { get; init; }
    }
}