using System.ComponentModel.DataAnnotations;

namespace booking_service.Models;

public class Booking
{
    public int BookingId{get;set;}
    [Required]
    public string BusId{get;set;}
    [Required]
    public int UserId{get;set;}
    public DateTime Date { get; set; }
    public int NumberOfSeats { get; set; }
    public List<int> BookedSeats { get; set; } = new List<int>();
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    
    
}