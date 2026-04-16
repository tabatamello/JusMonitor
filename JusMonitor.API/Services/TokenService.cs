using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JusMonitor.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace JusMonitor.API.Services;

public class TokenService(IConfiguration configuration)
{
    public string GerarToken(Advogado advogado)
    {
        var secret = configuration["Jwt:Secret"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, advogado.Id.ToString()),
            new Claim(ClaimTypes.Email, advogado.Email),
            new Claim(ClaimTypes.Name, advogado.Nome),
            new Claim("oab", advogado.NumeroOAB),
            new Claim("oab_validada", advogado.OABValidada.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}