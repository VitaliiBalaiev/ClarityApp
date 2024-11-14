using ClarityApp.API.DTOs;
using ClarityApp.API.Models;

namespace ClarityApp.API.Interfaces;

public interface IChatService
{
	Task StoreChatAsync(string chatId);
	Task StoreUserChatAsync(string recipientUser, string initiatorUser, string groupName);
	Task<List<string>> GetUserChatsAsync(string username);
}