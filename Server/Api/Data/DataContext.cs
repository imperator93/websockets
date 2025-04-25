using Microsoft.EntityFrameworkCore;

using Api.Models;

namespace Api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
    public required DbSet<User> Users { get; set; }
    public required DbSet<Message> Messages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasMany(u => u.Messages).WithOne(m => m.User).HasForeignKey(m => m.Id).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Message>().HasOne(m => m.User).WithMany(u => u.Messages);
    }
}
