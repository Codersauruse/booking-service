namespace booking_service.Exceptions;

public class InvalidArgumentException :Exception
{
    public InvalidArgumentException (string message) : base( message){}
}