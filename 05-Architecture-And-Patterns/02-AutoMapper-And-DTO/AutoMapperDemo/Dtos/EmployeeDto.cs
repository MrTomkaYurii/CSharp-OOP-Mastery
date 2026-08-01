namespace AutoMapperDemo.Dtos
{
    public class EmployeeDto
    {
        public string Name { get; set; } = "";
        public AddressDto? WorkAddress { get; set; }
        public bool IsHighEarner { get; set; }
    }
}