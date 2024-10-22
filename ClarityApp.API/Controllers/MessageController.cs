using System.Threading.Tasks;
using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClarityApp.API.Controllers;

[ApiController]
[Route("api/message")]
public class MessageController(IMessageService messageService) : ControllerBase
{
	[HttpGet("{chatId}")]
	public async Task<IActionResult> GetMessages(string chatId)
	{
		var messages = await messageService.GetAllMessagesAsync(chatId);
		return Ok(messages);
	}
}
