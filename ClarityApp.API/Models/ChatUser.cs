namespace ClarityApp.API.Models;

public class ChatUser
{
	public int ChatId { get; set; }
	public int UserId { get; set; }
	
	public Chat Chat { get; set; }
	public User User { get; set; }
}