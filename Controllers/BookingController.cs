using booking_service.DTO;
using booking_service.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace booking_service.Controllers;
[ApiController]
[Route("/api/v1/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    [HttpGet("/getAllBookings/{userId}")]
    public async Task<IActionResult> GetAllBookings(int userId)
    {
        var bookings = await _bookingService.GetAllBookingAsync(userId);
        return Ok(bookings);
    }
    
    
    [HttpGet("/getBooking/{bookingId}")]
    public async Task<IActionResult> GetBooking(int bookingId)
    {
        var bookings = await _bookingService.GetBookingByIdAsync(bookingId);
        return Ok(bookings);
    }
    
    
    [HttpPost("/addtoBooking")]
    public async Task<IActionResult> AddtoBooking([FromQuery] int userId, [FromQuery] string busId)
    {
        var bookings = await _bookingService.AddtoBookingAsync(userId, busId);
        return Ok(bookings);
    }
    
    [HttpPost("/cancelBooking")]
    public async Task<IActionResult> cancelBooking([FromQuery] int bookingId)
    {
        await _bookingService.CancelBookingAsync(bookingId);
        return Ok("you have cancelled the booking");
    }
    
    [HttpDelete("/deleteBooking/{bookingId}")]
    public async Task<IActionResult> deleteBooking(int bookingId)
    {
        await _bookingService.DeleteBookingAsync(bookingId);
        return Ok("you have deleted the booking");
    }

    [HttpPut("/confirmBooking/{bookingId}")]
    public async Task<IActionResult> ConfirmBooking(int bookingId, [FromBody] ConfirmBookingRequest request)
    {
        var booking = await _bookingService.CreateBookingAsync(bookingId, request.Date, request.NumberOfSeats, request.BookedSeats);
        return Ok(booking);
    }
}