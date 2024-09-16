namespace ClarityApp.API.Models;

public class AppUser
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public ICollection<Chat> Chats { get; set; }
}