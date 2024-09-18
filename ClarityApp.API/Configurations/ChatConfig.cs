using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClarityApp.API.Configurations;

public class ChatConfig : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chat");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Id).IsRequired();
        builder.Property(c => c.ChatName).IsRequired();
        
        builder.HasMany(c => c.Users)
            .WithMany(u => u.Chats);
        
        builder.HasMany(c => c.ChatUsers)
            .WithOne(cu => cu.Chat)
            .HasForeignKey(cu => cu.ChatId);
    }
}