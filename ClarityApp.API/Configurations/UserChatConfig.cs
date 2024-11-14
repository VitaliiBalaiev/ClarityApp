using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClarityApp.API.Configurations;
public class UserChatConfig : IEntityTypeConfiguration<UserChat>
{
	public void Configure(EntityTypeBuilder<UserChat> builder)
	{
		builder.ToTable("UserChat");
		
		builder.HasKey(uc => new { uc.Username, uc.ChatId });
		
		builder.HasIndex(uc => new { uc.Username, uc.ChatId }).IsUnique();
		
		builder.HasOne(uc => uc.Chat)
			.WithMany(c => c.UserChats)
			.HasForeignKey(uc => uc.ChatId)
			.OnDelete(DeleteBehavior.Cascade);
		
		builder.HasOne(cu => cu.User)
			.WithMany(u => u.UserChats)
			.HasForeignKey(uc => uc.Username)
			.HasPrincipalKey(u => u.UserName)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
