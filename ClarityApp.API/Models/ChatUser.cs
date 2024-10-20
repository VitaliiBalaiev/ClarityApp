namespace ClarityApp.API.Models;

public class ChatUser
{
	public string ChatId { get; set; }
	public Chat Chat { get; set; }
	
	public int UserId { get; set; }
	public User User { get; set; }
}