using System.Security.Cryptography;
using System.Text;
using ClarityApp.API.Data;
using ClarityApp.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Controllers;

public class AccountController : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(DataContext context, RegisterDTO registerDto)
    {
        if (await UserExists(context, registerDto.Username))
            return BadRequest("Username already exists");

        using var hmac = new HMACSHA512();

        var user = new User
        {
            UserName = registerDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        return user;
    }

    private async Task<bool> UserExists(DataContext context, string username)
    {
        return await context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}