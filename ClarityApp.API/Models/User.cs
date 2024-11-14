using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClarityApp.API.Models;

public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    public ICollection<Chat> Chats { get; set; }
    public ICollection<UserMessage> Messages { get; set; }
    public ICollection<UserChat> UserChats { get; set; }
}