using System;

namespace ClarityApp.API.Models;

public class UserMessage
{
    public int Id { get; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
    public string ChatId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
    //Navigation properties
    public Chat Chat { get; set; }
    public User Sender { get; set; }
}