using booking_service.Models;
using Microsoft.EntityFrameworkCore;
namespace booking_service.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Booking> Bookings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .Property(b => b.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<Booking>()
            .Property(b => b.BookedSeats)
            .HasConversion(
                v => string.Join(',', v), // Convert List<int> to "1,2,3"
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() // Convert back to List<int>
            );
    }
}