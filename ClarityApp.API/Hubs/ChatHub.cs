using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ClarityApp.API.DTOs;
using ClarityApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ClarityApp.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
	private static readonly Dictionary<string, string> _connections = new();
	private IMessageService _messageService;

	public ChatHub(IMessageService messageService)
	{
		_messageService = messageService;
	}
	public override Task OnConnectedAsync()
	{
		var username = Context.User?.Identity?.Name;
		if (!_connections.ContainsKey(username))
		{
			_connections.Add(username, Context.ConnectionId);
			Console.WriteLine($"{username} is connected. ConnectionId: {Context.ConnectionId}");
		}
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		var username = Context.User?.Identity?.Name;
		_connections.Remove(username);
		return base.OnDisconnectedAsync(exception);
	}
	
	public async Task SendMessage(string groupName, MessageDTO message)
	{
		Console.WriteLine($"{message.ChatId}, {message.SenderId}, {message.Content}, {message.Timestamp}, {message.SenderUsername}");
		await _messageService.StoreMessageAsync(message);
		await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
	}

	public async Task ShowChat(string initiatorUser, string recipientUser)
	{
		var groupName = CreateGroupName(initiatorUser, recipientUser);

		if (_connections.TryGetValue(initiatorUser, out var initiatorConnectionId))
		{
			await Groups.AddToGroupAsync(initiatorConnectionId, groupName);
			Console.WriteLine($"{initiatorUser} added to group {groupName}");
		}

		if (_connections.TryGetValue(recipientUser, out var recipientConnectionId))
		{
			await Groups.AddToGroupAsync(recipientConnectionId, groupName);
			Console.WriteLine($"{recipientUser} added to group {groupName}");
		}
	}

	private string CreateGroupName(string user1, string user2)
	{
		var users = new[] { user1, user2 };
		Array.Sort(users);
		return $"{users[0]}_{users[1]}_chat";
	}

}