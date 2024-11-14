using System.Data.Common;
using ClarityApp.API.Data;
using ClarityApp.API.Interfaces;
using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Services;

public class ChatService(DataContext context) : IChatService
{
	public async Task StoreChatAsync(string chatId)
	{
		if (!context.Chats.Any(chat => chat.Id.ToString() == chatId))
		{
			try
			{
				var chat = new Chat
				{
					Id = chatId
				};

				await context.Chats.AddAsync(chat);
				await context.SaveChangesAsync();
			}
			catch (DbException ex)
			{
				Console.WriteLine($"Error while saving data: {ex.Message}");
			}
			
		}
		else
		{
			Console.WriteLine("This chat already exists.");
		}
	}

	public async Task StoreUserChatAsync(string initiatorUser, string recipientUser, string groupName)
	{
		if (!context.UserChats.Any(uc =>
			    uc.ChatId == groupName &&
			    (uc.Username == initiatorUser || uc.Username == recipientUser)))
		{
			try
			{
				var userChats = new List<UserChat>
				{
					new UserChat
					{
						ChatId = groupName,
						Username = initiatorUser
					},
					new UserChat
					{
						ChatId = groupName,
						Username = recipientUser
					}
				};

				await context.UserChats.AddRangeAsync(userChats);
				await context.SaveChangesAsync();
			}
			catch (DbException ex)
			{
				Console.WriteLine($"Error while saving data: {ex.Message}");
			}
		}
		else
		{
			Console.WriteLine("This pair of Chat and Users already exist.");
		}
	}

	public async Task<List<string>> GetUserChatsAsync(string username)
	{
		var userChatIds = await context.UserChats
			.Where(uc => uc.Username == username)
			.Select(uc => uc.ChatId)
			.ToListAsync();
        
		var userChatList = await context.UserChats
			.Where(uc => userChatIds.Contains(uc.ChatId) && uc.Username != username)
			.Select(uc => uc.Username)
			.Distinct()
			.ToListAsync();

		return userChatList;
	}
}