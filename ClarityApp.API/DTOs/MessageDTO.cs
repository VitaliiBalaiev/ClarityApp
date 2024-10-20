namespace ClarityApp.API.DTOs;
public class MessageDTO
{
	public string ChatId { get; set; }
	public string SenderId { get; set; }
	public string SenderUsername { get; set; }
	public string Content { get; set; }
	public DateTime Timestamp { get; set; }
}