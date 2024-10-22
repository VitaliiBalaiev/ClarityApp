using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClarityApp.API.Controllers;

[ApiController]
[Route("api/chats")]
public class ChatsController(IChatService chatService) : ControllerBase
{
	[HttpPost("{chatId}")]
	public async Task<IActionResult> StoreChat(string chatId)
	{
		await chatService.StoreChatAsync(chatId);
		return Ok();
	}
}