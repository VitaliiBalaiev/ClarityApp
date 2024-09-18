using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClarityApp.API.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("AppUsers");

        // Primary key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(30);

        // Define one-to-many relationship with ChatUser (junction table)
        builder.HasMany(u => u.ChatUsers)     // A user has many ChatUser entries
            .WithOne(cu => cu.User)           // Each ChatUser has one User
            .HasForeignKey(cu => cu.UserId);  // FK for User
    }
}