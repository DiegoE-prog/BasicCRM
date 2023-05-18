namespace BasicCRM.Data.Entities
{
    public class Client
    {
        public Guid ClientID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid AddressID { get; set; }
    }
}
