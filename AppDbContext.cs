using Microsoft.EntityFrameworkCore;
using MedicalAPI.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map List<string> Permissions to PostgreSQL text[] column
        modelBuilder.Entity<Role>()
            .Property(r => r.Permissions)
            .HasColumnType("text[]");
    }
}
