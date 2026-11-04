using FeatureHubSDK;

namespace SubscriberService.ReleaseToggles;
public static class FeatureHubExtensions
{
    public static async Task<IServiceCollection> AddFeatureHubAsync(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<FeatureHubOptions>()
            .Bind(configuration.GetSection(FeatureHubOptions.SectionName))
            .Validate(options =>
                !string.IsNullOrWhiteSpace(options.EdgeUrl) &&
                !string.IsNullOrWhiteSpace(options.SdkKey),
                "FeatureHub configuration is incomplete.")
            .ValidateOnStart();

        var options = configuration
            .GetSection(FeatureHubOptions.SectionName)
            .Get<FeatureHubOptions>()
            ?? throw new InvalidOperationException(
                "FeatureHub configuration is missing.");

        var config = new EdgeFeatureHubConfig(
            edgeUrl: options.EdgeUrl,
            sdkKey: options.SdkKey);

        var featureHub = await config.NewContext().Build();

        services.AddSingleton<IClientContext>(featureHub);

        return services;
    }
}