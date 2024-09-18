using System.ComponentModel.DataAnnotations.Schema;

namespace ClarityApp.API.Models;

public class User
{
    public int Id { get; set; }
    public string? UserName { get; }

    [NotMapped] public bool IsOnline { get; set; } = true;
    
    public ICollection<Chat> Chats { get; }
    public ICollection<UserMessage> Messages { get; }
    public ICollection<ChatUser> ChatUsers { get; }
}