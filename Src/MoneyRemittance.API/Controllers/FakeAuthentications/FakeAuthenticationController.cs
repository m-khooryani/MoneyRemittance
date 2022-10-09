using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoneyRemittance.API.Configuration.Authentication;
using MoneyRemittance.API.Configuration.Authorization;

namespace MoneyRemittance.API.Controllers.FakeAuthentications;

[Route("api/fake-authentication")]
[ApiController]
public class FakeAuthenticationController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(FakeAuthenticationResponse), statusCode: 200)]
    [AllowAnonymous]
    [NoPermissionRequired]
    public IActionResult Login()
    {
        var claims = new List<Claim>
        {
            new Claim("Username", Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddYears(10),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(AuthenticationConstants.Key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return Ok(token);
    }
}
