using Microsoft.AspNetCore.Mvc;
using PcMate.Api.Data;
namespace PcMate.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public IActionResult Get() => Ok(_db.Products.Select(p => new { p.Id, p.Sku, p.Title, p.Subtitle, p.Price, p.Stock }));
}