using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ClarityApp.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
	private static readonly Dictionary<string, string> _connections = new();

	public override Task OnConnectedAsync()
	{
		var username = Context.User.Identity.Name;
		if (!_connections.ContainsKey(username))
		{
			_connections.Add(username, Context.ConnectionId);
			Console.WriteLine($"{username} is connected. ConnectionId: {Context.ConnectionId}");
		}
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		var username = Context.User.Identity.Name;
		_connections.Remove(username);
		return base.OnDisconnectedAsync(exception);
	}
	
	public async Task SendMessage(string receivingUsername, string message)
	{
		if (_connections.TryGetValue(receivingUsername, out var connectionId))
		{
			await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
		}
	}
	

}