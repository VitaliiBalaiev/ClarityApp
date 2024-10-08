using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ClarityApp.API.Hubs;

public class ChatHub : Hub
{
	public async Task SendMessage(string user, string message)
	{
		await Clients.All.SendAsync("ReceiveMessage", user, message);
	}

	public async Task JoinRoom(string roomName)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
		await Clients.Group(roomName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has joined the room {roomName}.");
	}

	public async Task LeaveRoom(string roomName)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
		await Clients.Group(roomName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has left the room {roomName}.");
	}
}