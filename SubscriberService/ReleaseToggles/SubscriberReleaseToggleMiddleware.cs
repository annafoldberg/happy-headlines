using FeatureHubSDK;

namespace SubscriberService.ReleaseToggles;

public class SubscriberReleaseToggleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IClientContext _featureHub;

    public SubscriberReleaseToggleMiddleware(RequestDelegate next, IClientContext featureHub)
    {
        _next = next;
        _featureHub = featureHub;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_featureHub["SubscriberServiceEnabled"].IsEnabled)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return;
        }

        await _next(context);
    }
}