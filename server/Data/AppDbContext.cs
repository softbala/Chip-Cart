using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PcMate.Api.Models;
namespace PcMate.Api.Data;
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) {}
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
}