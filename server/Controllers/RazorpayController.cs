using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
namespace PcMate.Api.Controllers;
[ApiController]
[Route("api/razorpay")]
public class RazorpayController : ControllerBase
{
    private readonly IConfiguration _cfg;
    public RazorpayController(IConfiguration cfg) { _cfg = cfg; }

    [HttpPost("order")]
    public IActionResult CreateOrder([FromBody] dynamic body)
    {
        int amount = (int)body.amount;
        var client = new RazorpayClient(_cfg["Razorpay:KeyId"], _cfg["Razorpay:KeySecret"]);
        var options = new Dictionary<string, object> {
            { "amount", amount },
            { "currency", "INR" },
            { "receipt", "rcpt_" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() },
            { "payment_capture", 1 }
        };
        var order = client.Order.Create(options);
        return Ok(order);
    }
}