namespace ClarityApp.API.Models;

public class UserChat
{
	public string ChatId { get; set; }
	public Chat Chat { get; set; }
	
	public string Username { get; set; }
	public User User { get; set; }
}