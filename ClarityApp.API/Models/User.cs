namespace ClarityApp.API.Models;

public class User
{
    public int Id { get; set; }
    public string? UserName { get; }
    
    public bool IsOnline { get; set; }
    public ICollection<Chat> Chats { get; }
    public ICollection<UserMessage> Messages { get; }
    public ICollection<ChatUser> ChatUsers { get; }
}