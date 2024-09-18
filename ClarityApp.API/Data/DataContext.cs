using ClarityApp.API.Configurations;
using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new ChatConfig());
        
    }
    
}