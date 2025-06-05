using booking_service.Exceptions;
using booking_service.Models;
using booking_service.Repository.Interfaces;
using booking_service.Service.Interfaces;

namespace booking_service.Service;

public class BookingService : IBookingService
{
    private readonly IBookingRepo _bookingRepo;
    private readonly ICommunicationService _communicationService;

    public BookingService(IBookingRepo bookingRepo, ICommunicationService communicationService)
    {
        _bookingRepo = bookingRepo;
        _communicationService = communicationService;
    }
    
    public async Task<IEnumerable<Booking>> GetAllBookingAsync(int userId)
    {
        var bookings = await _bookingRepo.GetAllBookingAsync(userId);

        if (bookings == null || !bookings.Any())
        {
            throw new NotFoundException($"No bookings found for userId: {userId}");
        }

        return bookings;
    }

    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        var existingBooking = await _bookingRepo.GetBookingByIdAsync(id);

        if (existingBooking == null)
        {
            throw new NotFoundException($"No bookings found for booking id: {id}");
        }

        return existingBooking ;
    }

    public async Task<Booking> AddtoBookingAsync(int userId, string busId)
    {  
        var isUserValid = await _communicationService.validateUserById(userId);
        var isBusValid =  await _communicationService.validateBusById(busId);
        
        if (!isBusValid)
        {
           throw new InvalidArgumentException("busId is not valid"); 
        }
        
        if (!isUserValid)
        {
           throw new InvalidArgumentException("userId is not valid"); 
        }
        
        return await _bookingRepo.AddToBookingAsync(userId, busId);
    }

    public async Task<Booking> CreateBookingAsync(int bookingId, DateTime date, int numberOfSeats, List<int> bookedSeats)
    {
        var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            // Validation: date can't be in the past
            if (date.Date < DateTime.Now.Date || date == null)
                throw new  InvalidArgumentException("Booking date cannot be in the past.");

            // Validate numberOfSeats
            if (numberOfSeats <= 0)
                throw new  InvalidArgumentException("Number of seats must be greater than zero.");

            // Validate bookedSeats list
            if (bookedSeats == null || bookedSeats.Count == 0)
                throw new  InvalidArgumentException("Booked seats list cannot be empty.");

            if (bookedSeats.Count != numberOfSeats)
                throw new  InvalidArgumentException("Number of booked seats does not match the number of seats.");


            return  await _bookingRepo.ConfirmBookingAsync(bookingId, date, numberOfSeats, bookedSeats);
        
    }

    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        bool isCanceled = await _bookingRepo.CancelBookingAsync(bookingId);
        if(!isCanceled)
        {
            throw new NotFoundException($"No bookings found for booking id: {bookingId}");
        }
        return isCanceled;
      
    }

    public async Task<bool> DeleteBookingAsync(int bookingId)
    {
        bool isDeleted = await _bookingRepo.DeleteBookingAsync(bookingId);
        if(!isDeleted)
        {
            throw new NotFoundException($"No bookings found for booking id: {bookingId}");
        }
        return isDeleted;
    }
}
