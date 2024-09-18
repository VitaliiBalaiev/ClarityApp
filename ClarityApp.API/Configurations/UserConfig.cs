using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClarityApp.API.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("AppUsers");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(30);

        builder.HasMany(u => u.Chats)
            .WithMany(c => c.Users);
        
        builder.HasMany(u => u.ChatUsers)
            .WithOne(cu => cu.User)
            .HasForeignKey(cu => cu.UserId);

    }
}