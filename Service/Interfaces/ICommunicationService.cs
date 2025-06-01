namespace booking_service.Service.Interfaces;

public interface ICommunicationService
{
    public Task<bool> validateUserById(int userId);
    public Task<bool> validateBusById(string busId);   
}