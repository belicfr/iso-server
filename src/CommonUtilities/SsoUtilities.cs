using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CommonUtilities;

public class SsoUtilities
{
    private readonly string _signingKey;
    private readonly IConfiguration _configuration;

    public SsoUtilities(IConfiguration configuration)
    {
        _signingKey = configuration["JWTSigningKey"]
                      ?? throw new InvalidOperationException("SigningKey is required.");
        
        _configuration = configuration;
    }

    public string GenerateJwtToken(string sso)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, sso),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["JwtIssuer"],
            audience: _configuration["JwtIssuer"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}