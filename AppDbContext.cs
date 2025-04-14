using MedicalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Add DbSet properties for each entity that you want to map to a database table.
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MasterPatientIndex> MasterPatientIndices { get; set; } // Add this line
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "admin", Permissions = new string[] { "superuser", "readonly" }, CreatedAt = DateTime.UtcNow }, // Add Permissions
                new Role { Id = 2, Name = "guest", Permissions = new string[] { "readonly" }, CreatedAt = DateTime.UtcNow }    // Add Permissions
            );

             modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            // Keep the original Role configuration.
            modelBuilder.Entity<Role>()
                .Property(r => r.Permissions)
                .HasColumnType("text[]");
        }
    }
}


