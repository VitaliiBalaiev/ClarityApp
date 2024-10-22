using System.Collections.Generic;

namespace ClarityApp.API.Models;

public class Chat
{
    public string Id { get; set; }
    
    public ICollection<User> Users { get; set; }
    public ICollection<UserMessage> Messages { get; set; }
    public ICollection<ChatUser> ChatUsers { get; set; }
    
}