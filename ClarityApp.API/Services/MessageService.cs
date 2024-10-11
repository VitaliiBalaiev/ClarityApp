using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClarityApp.API.Data;
using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Services;
public class MessageService : IMessageService
{
	private readonly DataContext _context;

	public MessageService(DataContext context)
	{
		_context = context;
	}

	public async Task StoreMessageAsync(MessageDTO messageDto)
	{
		var message = new UserMessage
		{
			ChatId = int.Parse(messageDto.ChatId),
			UserId = int.Parse(messageDto.UserId),
			Content = messageDto.Content,
			Timestamp = messageDto.Timestamp,
		};

		_context.Messages.Add(message);
		await _context.SaveChangesAsync();
	}

	public async Task<List<UserMessage>> GetAllMessagesAsync(int chatId)
	{
		return await _context.Messages
					.Where(m => m.ChatId == chatId)
					.OrderBy(m => m.Timestamp)
					.ToListAsync();
	}
}