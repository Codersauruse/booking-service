using booking_service.Data;
using booking_service.Models;
using booking_service.Repository.Interfaces;
using booking_service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace booking_service.Repository;

public class BookingRepo : IBookingRepo
{
    private readonly AppDbContext _context;
    private readonly ISeatRequestService _seatRequestService;

    public BookingRepo(AppDbContext context, ISeatRequestService seatRequestService)
    {
        _context = context;
        _seatRequestService = seatRequestService;
    }
    
    
    public  Task<Booking> AddToBookingAsync(int userId, string busId)
    {
        var booking = new Booking
        {
            UserId = userId,
            BusId = busId
        };
       
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return Task.FromResult(booking);
    }

    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        return booking;
    }

    public async Task<IEnumerable<Booking>> GetAllBookingAsync(int userId)
    {
        return await _context.Bookings
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return false;
        booking.Status = BookingStatus.Cancelled;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return false;

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }

    public  async Task<Booking?> ConfirmBookingAsync(int bookingId, DateTime date, int numberOfSeats, List<int> bookedSeats)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return null;

        booking.Date = date;
        booking.NumberOfSeats = numberOfSeats;
        booking.BookedSeats = bookedSeats;
        booking.Status = BookingStatus.Confirmed;
        
        _context.Bookings.Update(booking);
        await _seatRequestService.PublishMessage(booking);
        await _context.SaveChangesAsync();

        return booking;
    }
}