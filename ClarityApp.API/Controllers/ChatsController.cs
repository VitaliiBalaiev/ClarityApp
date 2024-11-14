using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using ClarityApp.API.Models;
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

	[HttpGet("{username}")]
	public async Task<IActionResult> GetChats(string username)
	{
		var chats = await chatService.GetUserChatsAsync(username);
		return Ok(chats);
	}
}