using ClarityApp.API.Data;
using ClarityApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Controllers;

[ApiController] 
[Route("api/[controller]")]

public class UsersController : ControllerBase
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