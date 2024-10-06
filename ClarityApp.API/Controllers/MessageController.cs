using System.Threading.Tasks;
using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClarityApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
	private readonly IMessageService _messageService;

	public MessageController(IMessageService messageService)
	{
		_messageService = messageService;
	}

	[HttpPost]
	public async Task<IActionResult> StoreMessage([FromBody] MessageDTO messageDto)
	{
		await _messageService.StoreMessageAsync(messageDto);
		return Ok();
	}

	[HttpGet("{chatId}")]
	public async Task<IActionResult> GetMessages(int chatId)
	{
		var messages = await _messageService.GetAllMessagesAsync(chatId);
		return Ok(messages);
	}
}
