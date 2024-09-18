using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClarityApp.API.Configurations;
public class ChatUserConfig : IEntityTypeConfiguration<ChatUser>
{
	public void Configure(EntityTypeBuilder<ChatUser> builder)
	{
		builder.ToTable("ChatUsers");

		builder.HasKey(cu => new { cu.ChatId, cu.UserId });

		builder.HasOne(cu => cu.Chat)
			.WithMany(c => c.ChatUsers)
			.HasForeignKey(cu => cu.ChatId) 
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(cu => cu.User)
			.WithMany(u => u.ChatUsers)    
			.HasForeignKey(cu => cu.UserId) 
			.OnDelete(DeleteBehavior.Cascade); 
	}
}
