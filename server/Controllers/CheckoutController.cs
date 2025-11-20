using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
namespace PcMate.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly IConfiguration _cfg;
    public CheckoutController(IConfiguration cfg) { _cfg = cfg; }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] dynamic body)
    {
        StripeConfiguration.ApiKey = _cfg["Stripe:SecretKey"] ?? "";
        var items = body.items;
        var lineItems = new List<SessionLineItemOptions>();
        foreach (var it in items)
        {
            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "inr",
                    ProductData = new SessionLineItemPriceDataProductDataOptions { Name = (string)it.title },
                    UnitAmount = (long)it.price
                },
                Quantity = (long)it.qty
            });
        }
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = (_cfg["BaseUrl"] ?? "http://localhost:5000") + "/success",
            CancelUrl = (_cfg["BaseUrl"] ?? "http://localhost:5000") + "/cart"
        };
        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return Ok(new { sessionId = session.Id, publishableKey = _cfg["Stripe:PublishableKey"] });
    }
}