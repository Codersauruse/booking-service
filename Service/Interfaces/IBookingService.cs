using booking_service.Models;

namespace booking_service.Service.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<Booking>> GetAllBookingAsync(int userId);
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<Booking> AddtoBookingAsync(int userId, string busId);
    Task<Booking> CreateBookingAsync(int bookingId, DateTime date, int numberOfSeats, List<int> bookedSeats);
    Task<bool> CancelBookingAsync(int bookingId);
    Task<bool> DeleteBookingAsync(int bookingId);
}