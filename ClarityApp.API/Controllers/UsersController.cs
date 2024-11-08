using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClarityApp.API.Data;
using ClarityApp.API.DTOs;
using ClarityApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Controllers;

[Authorize]
[ApiController] 
[Route("api/users")]

public class UsersController : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<User>>> GetUsers(DataContext context)
	{
		return await context.Users.ToListAsync();
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<User>> GetUser(DataContext context, int id)
	{
		return await context.Users.FindAsync(id);
	}
    
	[HttpGet("{username}")]
	public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserByUsername(string? username, DataContext context)
	{
		if (username != null)
		{
			return await context.Users
				.Where(u => EF.Functions.Like(u.UserName, $"%{username}%"))
				.Select(u => new UserDTO { Username = u.UserName })
				.ToListAsync();
            
		}

		return await Task.FromResult(Array.Empty<UserDTO>().ToList());

	}
}