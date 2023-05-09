using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Dtos.AddressDto
{
    public record AddressToUpdateDto
    {
        public Guid AddressID { get; init; }
        public string AddressLine { get; init; } = string.Empty;
        public string? AddressDetails { get; init; }
        public string City { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public int ZipCode { get; init; }
        public string Country { get; init; } = string.Empty;
    }
}
