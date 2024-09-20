using ClarityApp.API.Data;
using ClarityApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Controllers;

public class UsersController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(DataContext context)
    {
        return await context.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(DataContext context, int id)
    {
	    return await context.Users.FindAsync(id);
    }
}