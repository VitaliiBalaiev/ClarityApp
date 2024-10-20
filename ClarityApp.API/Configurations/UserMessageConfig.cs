using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClarityApp.API.Configurations;

public class UserMessageConfig : IEntityTypeConfiguration<UserMessage>
{
    public void Configure(EntityTypeBuilder<UserMessage> builder)
    {
        builder.ToTable("UserMessages");
        
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Id).IsRequired();
        builder.Property(m => m.Content).IsRequired().HasMaxLength(1024);

        builder.HasOne(m => m.Chat)
            .WithMany(c => c.Messages);
        
        builder.HasOne(m => m.Sender)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.SenderId);


    }
}