using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Hosting;

namespace Monitoring;

public static class MonitoringExtensions
{
    public static IServiceCollection AddMonitoring(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services
            .AddOptions<SeqOptions>()
            .Bind(configuration.GetSection(SeqOptions.SectionName))
            .Validate(options =>
                !string.IsNullOrWhiteSpace(options.SeqUrl),
                "Seq configuration is incomplete.")
            .ValidateOnStart();

        // https://github.com/serilog/serilog-aspnetcore
        services.AddSerilog((sp, lc) =>
        {
            var seqOptions = sp
                .GetRequiredService<IOptions<SeqOptions>>()
                .Value;

            lc
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(sp)
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seqOptions.SeqUrl);
        });

        // https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/trace/getting-started-aspnetcore/README.md
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(environment.ApplicationName)) // application that made the trace
            .WithTracing(tracing => tracing
                .AddSource(environment.ApplicationName) // producer of the custom spans
                .AddSource("Messaging.RabbitMQ")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://aspire-dashboard:18889");
                }));

        return services;
    }
}