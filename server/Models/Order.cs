using System.ComponentModel.DataAnnotations;
namespace PcMate.Api.Models;
public class Order {
  public int Id { get; set; }
  [Required] public string OrderId { get; set; } = string.Empty;
  public int Amount { get; set; }
  public string Currency { get; set; } = "INR";
  public string Status { get; set; } = "NEW";
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public string Items { get; set; } = "[]";
}