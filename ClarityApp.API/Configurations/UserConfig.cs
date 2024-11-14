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
        builder.HasIndex(u => u.UserName).IsUnique();

        // Properties
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(30);
    }
}