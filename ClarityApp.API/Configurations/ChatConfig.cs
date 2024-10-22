using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClarityApp.API.Configurations;

public class ChatConfig : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");

        // Primary key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Id).IsRequired();

        // Define one-to-many relationship with ChatUser (junction table)
        builder.HasMany(c => c.ChatUsers)     // A chat has many ChatUser entries
            .WithOne(cu => cu.Chat)           // Each ChatUser has one Chat
            .HasForeignKey(cu => cu.ChatId);  // FK for Chat
    }
}
