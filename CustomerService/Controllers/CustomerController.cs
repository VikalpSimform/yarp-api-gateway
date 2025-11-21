namespace CustomerService.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private static readonly List<Customer> Customers = new()
        {
            new Customer { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com", PhoneNumber = "123-456-7890", Address = "123 Main St" },
            new Customer { Id = 2, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", PhoneNumber = "234-567-8901", Address = "456 Oak Ave" },
            new Customer { Id = 3, FirstName = "Carol", LastName = "Williams", Email = "carol.williams@example.com", PhoneNumber = "345-678-9012", Address = "789 Pine Rd" }
        };

        [HttpGet("customer/public/get-all")]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return Ok(Customers);
        }

        [HttpGet("customer/api/get-customer-by-name")]
        public ActionResult<IEnumerable<Customer>> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name parameter cannot be null or empty.");
            }
            var filteredCustomers = Customers.Where(c => c.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) || c.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(filteredCustomers);
        }

        [HttpGet("customer/public/health")]
        public IActionResult Health() => Ok("Healthy");
    }
}
