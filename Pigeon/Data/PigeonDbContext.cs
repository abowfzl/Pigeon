using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pigeon.Entities;

namespace Pigeon.Data;

public class PigeonDbContext : IdentityDbContext
{
    public PigeonDbContext(DbContextOptions<PigeonDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<LogRequest> LogRequests { get; set; }

    //public DbSet<Store> Stores { get; set; }

    public DbSet<Url> Urls { get; set; }

    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Tickets");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<LogRequest>(entity =>
        {
            entity.ToTable("LogRequests");

            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Url>(entity =>
        {
            entity.ToTable("Urls");

            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tags");

            entity.HasKey(e => e.Id);
        });

        base.OnModelCreating(modelBuilder);
    }
}