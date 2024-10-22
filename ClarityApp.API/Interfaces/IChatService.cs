using ClarityApp.API.DTOs;

namespace ClarityApp.API.Interfaces;

public interface IChatService
{
	Task StoreChatAsync(string chatId);
}