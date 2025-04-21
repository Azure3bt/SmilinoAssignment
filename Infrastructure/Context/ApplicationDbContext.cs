using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;
public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserMessage> UserMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserMessage>()
            .HasKey(m => m.MessageId);

        builder.Entity<UserMessage>()
            .Property(m => m.MessageId)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Entity<UserMessage>()
            .Property(m => m.Message)
            .HasMaxLength(500);

        builder.Entity<UserMessage>()
            .HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId);

        builder.Entity<User>()
            .HasMany(u => u.Messages)
            .WithOne(u => u.User)
            .HasForeignKey(m => m.UserId)
            .IsRequired();
    }
}