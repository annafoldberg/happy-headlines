using System.Diagnostics;

namespace Monitoring;

public static class TracingSource
{
    public static readonly string ActivitySourceName = "HappyHeadlines";
    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}
