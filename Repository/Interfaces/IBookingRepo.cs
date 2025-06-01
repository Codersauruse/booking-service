using booking_service.Models;

namespace booking_service.Repository.Interfaces;

public interface IBookingRepo
{
    Task<Booking> AddToBookingAsync(int userId, string busId);
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<IEnumerable<Booking>> GetAllBookingAsync(int userId);
    Task<bool> CancelBookingAsync(int bookingId);
    Task<bool> DeleteBookingAsync(int bookingId);
    Task<Booking?> ConfirmBookingAsync(int bookingId, DateTime date, int numberOfSeats, List<int> bookedSeats);
}
