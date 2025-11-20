using Microsoft.AspNetCore.Mvc;
using PcMate.Api.Utils;
using System.Net.Http;
using System.Text.Json;
using System.Text;
namespace PcMate.Api.Controllers;
[ApiController]
[Route("api/paytm")]
public class PaytmController : ControllerBase
{
    private readonly IConfiguration _cfg;
    private readonly IHttpClientFactory _httpFactory;
    public PaytmController(IConfiguration cfg, IHttpClientFactory httpFactory) { _cfg = cfg; _httpFactory = httpFactory; }

    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate([FromBody] PaytmInitiateModel model)
    {
        var mid = _cfg["Paytm:Mid"] ?? "";
        var merchantKey = _cfg["Paytm:MerchantKey"] ?? "";
        var orderId = model.OrderId;
        var amount = model.Amount.ToString("F2");

        var paytmParams = new Dictionary<string, string>
        {
            { "MID", mid },
            { "ORDER_ID", orderId },
            { "CUST_ID", model.CustomerId ?? "CUST_1" },
            { "TXN_AMOUNT", amount },
            { "CHANNEL_ID", "WEB" },
            { "WEBSITE", _cfg["Paytm:Website"] ?? "WEBSTAGING" },
            { "INDUSTRY_TYPE_ID", _cfg["Paytm:Industry"] ?? "Retail" },
            { "CALLBACK_URL", (_cfg["BaseUrl"] ?? "http://localhost:5000") + "/api/paytm/callback" }
        };

        var checksum = PaytmChecksum.GenerateSignature(paytmParams, merchantKey);

        var requestBody = new {
            body = new {
                requestType = "Payment",
                mid = mid,
                websiteName = paytmParams["WEBSITE"],
                orderId = orderId,
                txnAmount = new { value = amount, currency = "INR" },
                userInfo = new { custId = paytmParams["CUST_ID"] },
                callbackUrl = paytmParams["CALLBACK_URL"]
            },
            head = new { signature = checksum }
        };

        var url = _cfg["Paytm:InitiateUrl"] ?? "https://securegw-stage.paytm.in/theia/api/v1/initiateTransaction?mid=" + mid + "&orderId=" + orderId;
        var client = _httpFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var resp = await client.PostAsync(url, content);
        var respText = await resp.Content.ReadAsStringAsync();
        return Ok(new { raw = respText });
    }

    [HttpPost("callback")]
    public IActionResult Callback()
    {
        return Ok("OK");
    }
}

public class PaytmInitiateModel { public string OrderId { get; set; } = string.Empty; public double Amount { get; set; } public string? CustomerId { get; set; } }