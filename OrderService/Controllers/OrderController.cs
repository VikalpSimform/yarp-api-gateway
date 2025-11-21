namespace OrderService.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    private static readonly List<Order> Orders =
    [
        new Order
        {
            Id = 1,
            CustomerName = "Alice Smith",
            OrderDate = DateTime.UtcNow.AddDays(-2),
            TotalAmount = 120.50m,
            Status = "Processing",
            ShippingAddress = "123 Main St, Cityville",
            Notes = "Leave at front door"
        },
        new Order
        {
            Id = 2,
            CustomerName = "Bob Johnson",
            OrderDate = DateTime.UtcNow.AddDays(-1),
            TotalAmount = 75.00m,
            Status = "Shipped",
            ShippingAddress = "456 Oak Ave, Townsville",
            Notes = null
        }
    ];

    [HttpGet("order/public/get-all")]
    public ActionResult<IEnumerable<Order>> GetOrders()
    {
        return Ok(Orders);
    }

    [HttpGet("order/api/get-admin-all")]
    public ActionResult<IEnumerable<Order>> GetAdminOrders()
    {
        return Ok(Orders);
    }

    [HttpGet("order/public/health")]
    public IActionResult Health() => Ok("Healthy");

    [HttpGet("order/public/transformation")]
    public IActionResult Transformation()
    {
        var calledFrom = Request.Headers["X-Called-From"].FirstOrDefault();
        var ts = Request.Headers["X-Requested-Service"].FirstOrDefault();
        var gatewayParam = HttpContext.Request.Query["gateway"].FirstOrDefault();

        return Ok(new
        {
            Status = "Healthy",
            X_Called_From = calledFrom,
            X_Request_Timestamp = ts,
            QueryParams = gatewayParam
        });
    }
}
