using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using PcMate.Api.Models;
namespace PcMate.Api.Controllers;
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _cfg;

    public AdminController(UserManager<ApplicationUser> userManager, IConfiguration cfg)
    {
        _userManager = userManager;
        _cfg = cfg;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null) return Unauthorized();
        if (!await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();

        var jwtKey = _cfg["Jwt:Key"] ?? "dev_secret_change_me";
        var issuer = _cfg["Jwt:Issuer"] ?? "pcmate";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: issuer, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = jwt });
    }
}

public class LoginModel { public string Username { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }