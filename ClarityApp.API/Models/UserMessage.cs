using System;

namespace ClarityApp.API.Models;

public class UserMessage
{
    public int Id { get; }
    public int UserId { get; set; }
    
    public int ChatId { get; set; }
    public string Content { get; set; }
    
    public DateTime Timestamp { get; set; }
    public Chat Chat { get; set; }
    public User Sender { get; set; }
}