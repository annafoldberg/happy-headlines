namespace SubscriberService.ReleaseToggles;

public class FeatureHubOptions
{
    public const string SectionName = "FeatureHub";
    public string EdgeUrl { get; set; } = string.Empty;
    public string SdkKey { get; set; } = string.Empty;
}