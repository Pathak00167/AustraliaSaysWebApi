using AustraliaSaysWebApi.DataAccess.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Articles> Articles { get; set; }
        public DbSet<Notification> Notification { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>()
              .HasMany(c => c.Articles)
              .WithOne(a => a.Category)
              .HasForeignKey(a => a.CategoryId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
          .HasMany(u => u.Notification)
          .WithOne(n => n.User)
          .HasForeignKey(n => n.UserId)
          .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
