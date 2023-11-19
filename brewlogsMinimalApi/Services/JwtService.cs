using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace brewlogsMinimalApi.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly string _issuer;
    private readonly string[] _validAudiences;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _issuer = _configuration["Authentication:ValidIssuer"] ??
                  throw new InvalidOperationException("ValidIssuer is not defined in appsettings.");
        _validAudiences = _configuration.GetSection("Authentication:ValidAudiences").Get<string[]>() ??
                          throw new InvalidOperationException("ValidAudiences is not defined in appsettings.");
    }

    public string GenerateJwtToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = _configuration["secretKey"];

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("Secret key is not defined in user secrets.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _issuer,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature),
            Audience = string.Join(",", _validAudiences)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}