using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PcMate.Api.Data;
using CsvHelper;
using System.Globalization;

namespace PcMate.Api.Controllers;
[ApiController]
[Route("api/admin")]
public class AdminUploadController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminUploadController(AppDbContext db) { _db = db; }

    [HttpPost("upload-csv")]
    [Authorize]
    public async Task<IActionResult> Upload()
    {
        if (!Request.HasFormContentType) return BadRequest("Expected form-data");
        var form = await Request.ReadFormAsync();
        var file = form.Files.GetFile("file");
        if (file == null) return BadRequest("No file");

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<dynamic>().ToList();
        foreach (var r in records)
        {
            var dict = (IDictionary<string, object>)r;
            var sku = dict.ContainsKey("sku") ? dict["sku"]?.ToString() ?? "" : "";
            var title = dict.ContainsKey("title") ? dict["title"]?.ToString() ?? "" : "";
            var price = dict.ContainsKey("price") ? Convert.ToDouble(dict["price"]) : 0;
            var stock = dict.ContainsKey("stock") ? Convert.ToInt32(dict["stock"]) : 0;
            if (string.IsNullOrEmpty(sku)) continue;
            var existing = _db.Products.FirstOrDefault(p => p.Sku == sku);
            if (existing != null)
            {
                existing.Title = title;
                existing.Price = (int)(price * 100);
                existing.Stock = stock;
            }
            else
            {
                _db.Products.Add(new Models.Product { Sku = sku, Title = title, Price = (int)(price * 100), Stock = stock });
            }
        }
        _db.SaveChanges();
        return Ok(new { count = records.Count });
    }
}