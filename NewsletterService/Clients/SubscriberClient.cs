using NewsletterService.Contracts;

namespace NewsletterService.Clients;

public class SubscriberClient : ISubscriberClient
{
    private readonly HttpClient _httpClient;

    public SubscriberClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<SubscriberResponse>> GetSubscribersAsync(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"subscribers", ct);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<SubscriberResponse>>(cancellationToken: ct);
        
        if (result is null)
            throw new InvalidOperationException("SubscriberService returned an empty response.");

        return result;
    }
}