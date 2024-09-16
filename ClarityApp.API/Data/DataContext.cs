using ClarityApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClarityApp.API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>().ToTable("Users");
    }
    
}