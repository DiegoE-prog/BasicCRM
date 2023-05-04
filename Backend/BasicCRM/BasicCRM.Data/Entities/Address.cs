namespace BasicCRM.Data.Entities
{
    public class Address
    {
        public Guid AddressID { get; set; }
        public string AddressLine { get; set; } = string.Empty;
        public string? AddressDetails { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public string Country { get; set; } = string.Empty;
    }
}
