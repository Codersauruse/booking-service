namespace booking_service.Service.Interfaces;

public interface ISeatRequestService
{
    public Task PublishMessage<T>(T message);
    public void Dispose();
}