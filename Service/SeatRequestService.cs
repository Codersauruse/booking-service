using System.Text;
using System.Text.Json;
using booking_service.Exceptions;
using booking_service.Service.Interfaces;
using RabbitMQ.Client;

namespace booking_service.Service;

public class SeatRequestService : ISeatRequestService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private const string SEAT_REQUEST_QUEUE = "seat-request";


    public SeatRequestService(IConnection connection)
    {
        _connection = connection;
        _channel = _connection.CreateChannelAsync().Result;
        _channel.QueueDeclareAsync(SEAT_REQUEST_QUEUE, durable: true, exclusive: false, autoDelete: false,
            arguments: null);
    }
    public async Task PublishMessage<T>(T message)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(exchange: "", routingKey: SEAT_REQUEST_QUEUE, body: body);
            Console.WriteLine("message sent successfully");

        }
        catch (Exception ex)
        {
            throw new MessageException(ex.Message);
            Console.WriteLine(ex.Message);
        }

    }

    public void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
    
    
}