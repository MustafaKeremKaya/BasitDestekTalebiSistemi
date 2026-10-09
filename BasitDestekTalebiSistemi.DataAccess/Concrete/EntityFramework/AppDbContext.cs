using System;
using Microsoft.EntityFrameworkCore;
using BasitDestekTalebiSistemi.Entities.Concrete;

namespace BasitDestekTalebiSistemi.DataAccess.Concrete.EntityFramework
{

    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; } = null!;

        public DbSet<Reply> Replies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TicketSystemDb;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Tickets");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(t => t.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(t => t.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(t => t.Status)
                    .IsRequired();

                entity.Property(t => t.CreatedAt)
                    .IsRequired();

                entity.HasMany(t => t.Replies)
                    .WithOne(r => r.Ticket)
                    .HasForeignKey(r => r.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Reply>(entity =>
            {
                entity.ToTable("Replies");

                entity.HasKey(r => r.Id);

                entity.Property(r => r.RepliedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(r => r.Content)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(r => r.CreatedAt)
                    .IsRequired();

                entity.HasIndex(r => r.TicketId);

                entity.HasIndex(r => r.CreatedAt);
            });
        }
    }
}
