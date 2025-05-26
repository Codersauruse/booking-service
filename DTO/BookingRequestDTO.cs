namespace booking_service.DTO;

public class ConfirmBookingRequest
{
    
    public DateTime Date { get; set; }
    public int NumberOfSeats { get; set; }
    public List<int> BookedSeats { get; set; } 
}