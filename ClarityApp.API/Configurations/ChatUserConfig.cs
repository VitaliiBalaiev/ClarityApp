using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Configurations;

public class ChatUserConfig : IEntityTypeConfiguration<ChatUser>
{
	public void Configure(EntityTypeBuilder<ChatUser> builder)
	{
		builder.ToTable("ChatUser");
		
		builder.Property(cu => cu.ChatId).IsRequired();
		builder.Property(cu => cu.UserId).IsRequired();
		
		builder.HasKey(cu => new {cu.ChatId, cu.UserId});

		builder.HasOne(cu => cu.Chat)
			.WithMany(cu => cu.ChatUsers)
			.HasForeignKey(cu => cu.ChatId);
		
		builder.HasOne(cu => cu.User)
			.WithMany(u => u.ChatUsers)
			.HasForeignKey(cu => cu.UserId);
	}
}