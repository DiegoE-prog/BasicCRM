using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Dtos.AddressDto
{
    public class GetAddressDto
    {
        public Guid AddressID { get; set; }
        public string AddressLine { get; set; }
        public string? AddressDetails { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public string Country { get; set; }
    }
}