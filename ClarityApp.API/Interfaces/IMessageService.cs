using System.Collections.Generic;
using System.Threading.Tasks;
using ClarityApp.API.Models;
using ClarityApp.API.DTOs;

namespace ClarityApp.API.Interfaces;
public interface IMessageService
{
	Task StoreMessageAsync(MessageDTO messageDto);
	Task<List<UserMessage>> GetAllMessagesAsync(int chatId);  // Change parameter type to string for ChatId
}