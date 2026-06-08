using Microsoft.EntityFrameworkCore;
using HKtab.Backend.Models;

namespace HKtab.Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<Income> Incomes { get; set; } = null!;
    public DbSet<Ledger> Ledgers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Expense configuration
        modelBuilder.Entity<Expense>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Income configuration
        modelBuilder.Entity<Income>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Income>()
            .HasOne(i => i.User)
            .WithMany(u => u.Incomes)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ledger configuration
        modelBuilder.Entity<Ledger>()
            .HasKey(l => l.Id);
        modelBuilder.Entity<Ledger>()
            .HasOne(l => l.User)
            .WithMany(u => u.Ledgers)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
