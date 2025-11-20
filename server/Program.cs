using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PcMate.Api.Data;
using PcMate.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=pcmate.db"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev_secret_change_me";
var issuer = builder.Configuration["Jwt:Issuer"] ?? "pcmate";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    if (!roleManager.RoleExistsAsync("Admin").Result)
    {
        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
    }

    var adminUserName = builder.Configuration["Admin:User"] ?? "admin";
    var adminEmail = builder.Configuration["Admin:Email"] ?? "admin@pcmate.local";
    var adminPass = builder.Configuration["Admin:Pass"] ?? "Password123!";

    var adminUser = userManager.FindByNameAsync(adminUserName).Result;
    if (adminUser == null)
    {
        var user = new ApplicationUser { UserName = adminUserName, Email = adminEmail, EmailConfirmed = true };
        var result = userManager.CreateAsync(user, adminPass).Result;
        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(user, "Admin").Wait();
        }
    }

    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new PcMate.Api.Models.Product{Sku="cpu-i7", Title="Intel Core i7 13700K", Subtitle="12 cores, 20 threads", Price=34900, Stock=10},
            new PcMate.Api.Models.Product{Sku="gpu-rtx4070", Title="NVIDIA RTX 4070", Subtitle="12GB GDDR6X", Price=59900, Stock=5},
            new PcMate.Api.Models.Product{Sku="ram-32gb", Title="Corsair Vengeance 32GB", Subtitle="DDR5 5600MHz", Price=9999, Stock=25},
            new PcMate.Api.Models.Product{Sku="ssd-1tb", Title="Samsung NVMe 1TB", Subtitle="Gen4 NVMe SSD", Price=7999, Stock=50}
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
