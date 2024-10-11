using System.Threading.Tasks;
using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClarityApp.API.Controllers;

[ApiController]
[Route("api/message")]
public class MessageController : ControllerBase
{
	private readonly IMessageService _messageService;

	public MessageController(IMessageService messageService)
	{
		_messageService = messageService;
	}

	[HttpGet("{chatId}")]
	public async Task<IActionResult> GetMessages(int chatId)
	{
		var messages = await _messageService.GetAllMessagesAsync(chatId);
		return Ok(messages);
	}
}
