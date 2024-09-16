namespace ClarityApp.API.Models;

public class UserMessage
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public Chat Chat { get; set; }
}