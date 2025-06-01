using System.Linq.Expressions;
using booking_service.Exceptions;
using booking_service.Service.Interfaces;
using Steeltoe.Discovery;

namespace booking_service.Service;

public class CommunicationService : ICommunicationService
{
    private readonly IDiscoveryClient _discoveryClient;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public CommunicationService(IDiscoveryClient discoveryClient,IHttpClientFactory httpClientFactory)
    {
        _discoveryClient = discoveryClient;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> validateUserById(int userId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var instances = _discoveryClient.GetInstances("user-service");
            if (!instances.Any())
            {
                return false;
            }
            var instance = instances.First();
            
            var url = $"{instance.Uri}/api/user/validateUser/{userId}";
            var response = await httpClient.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            throw new InvalidArgumentException("Invalid userId");

        }
    }

    public async Task<bool>  validateBusById(string busId)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var instances = _discoveryClient.GetInstances("timetable-service");
            if (!instances.Any())
            {
                return false;
            }
            var instance = instances.First();
            
            var url = $"{instance.Uri}/api/bus//validateBusId/{busId}";
            var response = await httpClient.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            throw new InvalidArgumentException("Invalid busId");

        }
    }
}