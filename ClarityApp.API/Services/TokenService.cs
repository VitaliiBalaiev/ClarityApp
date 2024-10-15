using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClarityApp.API.Interfaces;
using ClarityApp.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClarityApp.API.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly IConfiguration _config;
    
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:TokenKey"]));
    }
    
    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.UserName)
        };
        
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(5),
            SigningCredentials = creds,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}