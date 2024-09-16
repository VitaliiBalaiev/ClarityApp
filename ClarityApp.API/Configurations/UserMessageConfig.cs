using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClarityApp.API.Configurations;

public class UserMessageConfig : IEntityTypeConfiguration<UserMessage>
{
    public void Configure(EntityTypeBuilder<UserMessage> builder)
    {
        builder.ToTable("UserMessages");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id).IsRequired();
        
        builder.Property(u => u.Content).IsRequired().HasMaxLength(1024);
        

    }
}