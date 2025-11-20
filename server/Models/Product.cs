using System.ComponentModel.DataAnnotations;
namespace PcMate.Api.Models;
public class Product {
  public int Id { get; set; }
  [Required] public string Sku { get; set; } = string.Empty;
  [Required] public string Title { get; set; } = string.Empty;
  public string? Subtitle { get; set; }
  public int Price { get; set; }
  public int Stock { get; set; }
}