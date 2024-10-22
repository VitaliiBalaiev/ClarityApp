using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClarityApp.API.Data;
using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Services;
public class MessageService(DataContext context) : IMessageService
{
	public async Task StoreMessageAsync(MessageDTO messageDto)
	{
		var message = new UserMessage
		{
			ChatId = messageDto.ChatId,
			SenderId = int.Parse(messageDto.SenderId),
			Content = messageDto.Content,
			Timestamp = messageDto.Timestamp,
			SenderUsername = messageDto.SenderUsername
		};

		try
		{
			context.Messages.Add(message);
			await context.SaveChangesAsync();
		}
		catch (DbUpdateException ex)
		{
			Console.WriteLine($"Error saving message: {ex.Message}");
		}
	}

	public async Task<List<UserMessage>> GetAllMessagesAsync(string chatId)
	{
		return await context.Messages
					.Where(m => m.ChatId == chatId)
					.OrderBy(m => m.Timestamp)
					.ToListAsync();
	}
}