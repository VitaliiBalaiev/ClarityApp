using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ClarityApp.API.Data;
using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ClarityApp.API.Hubs;

[Authorize]
public class ChatHub(IMessageService messageService, IChatService chatService) : Hub
{
	private static readonly Dictionary<string, string> _connection = new();
	
	public override Task OnConnectedAsync()
	{
		var username = Context.User?.Identity?.Name;
		if (!_connection.ContainsKey(username))
		{
			_connection.Add(username, Context.ConnectionId);
			Console.WriteLine($"{username} is connected. ConnectionId: {Context.ConnectionId}");
		}
		else 
		{
			_connection[username] = Context.ConnectionId;
			Console.WriteLine($"{username} is connected with new connectionId: {Context.ConnectionId}");
		}

		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		var username = Context.User?.Identity?.Name;
		_connection.Remove(username);
		return base.OnDisconnectedAsync(exception);
	}
	
	public async Task SendMessage(string groupName, MessageDTO message)
	{
		Console.WriteLine($"{message.ChatId}, {message.SenderId}, {message.Content}, {message.Timestamp}, {message.SenderUsername}");
		await messageService.StoreMessageAsync(message);
		await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
	}

	public async Task ShowChat(string initiatorUser, string recipientUser)
    {
        try
        {
            var groupName = CreateGroupName(initiatorUser, recipientUser);
            
            await chatService.StoreChatAsync(groupName);
    
            if (_connection.TryGetValue(initiatorUser, out var initiatorConnectionId))
            {
                await Groups.AddToGroupAsync(initiatorConnectionId, groupName);
                Console.WriteLine($"{initiatorUser} added to group {groupName}");
            }
            else
            {
                Console.WriteLine($"Initiator {initiatorUser} not found in connections.");
            }
    
            if (_connection.TryGetValue(recipientUser, out var recipientConnectionId))
            {
                await Groups.AddToGroupAsync(recipientConnectionId, groupName);
                Console.WriteLine($"{recipientUser} added to group {groupName}");
            }
            else
            {
                Console.WriteLine($"Recipient {recipientUser} not found in connections.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ShowChat: {ex.Message}");
            throw;
        }
    }


	private string CreateGroupName(string user1, string user2)
	{
		var users = new[] { user1, user2 };
		Array.Sort(users);
		return $"{users[0]}_{users[1]}_chat";
	}
	

}