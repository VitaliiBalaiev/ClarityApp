namespace ClarityApp.API.Models;

public class Chat
{
    public int Id { get; set; }
    public string ChatName { get; set; }
    public ICollection<AppUser> Users { get; set; }
    public ICollection<UserMessage> Messages { get; set; }
    
}