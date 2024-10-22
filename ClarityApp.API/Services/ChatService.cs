using System.Data.Common;
using ClarityApp.API.Data;
using ClarityApp.API.Interfaces;
using ClarityApp.API.Models;

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
}