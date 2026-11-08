namespace SerializationDemo.Models
{
    public class Address
    {
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string Building { get; set; } = default!;
        public string? Apartment { get; set; }
        public string PostalCode { get; set; } = default!;
    }
}