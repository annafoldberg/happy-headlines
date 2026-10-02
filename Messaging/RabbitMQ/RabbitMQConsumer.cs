using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using OpenTelemetry.Context.Propagation;
using System.Text;
using System.Diagnostics;

namespace Messaging.RabbitMQ;

public sealed class RabbitMQConsumer : IEventConsumer
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;
    private string? _queue;
    private static readonly TextMapPropagator _propagator = new TraceContextPropagator();
    private static readonly ActivitySource _activitySource = new("Messaging.RabbitMQ");

    public RabbitMQConsumer(string host, string user, string pass, int port)
    {
        _factory = new ConnectionFactory
        {
            HostName = host,
            UserName = user,
            Password = pass,
            Port = port,
            AutomaticRecoveryEnabled = true
        };
    }

    public async Task SubscribeAsync(string queue, string exchange, CancellationToken ct)
    {
        _connection = await _factory.CreateConnectionAsync(ct);
        _channel = await _connection.CreateChannelAsync();

        // Declare the fanout exchange
        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout, durable: true, autoDelete: false);

        // Declare and bind the queue
        await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
        await _channel.QueueBindAsync(queue, exchange, "");

        _queue = queue;
    }

    public async Task RunAsync(Func<ReadOnlyMemory<byte>, CancellationToken, Task<bool>> handler, CancellationToken ct)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var parentContext = _propagator.Extract(
                default,
                ea.BasicProperties,
                static (properties, key) =>
                {
                    if (properties.Headers is null || !properties.Headers.TryGetValue(key, out var value))
                    {
                        return [];
                    }

                    return value is byte[] bytes ? [Encoding.UTF8.GetString(bytes)] : [];
                });

            using var activity = _activitySource.StartActivity(
                "Message received",
                ActivityKind.Consumer,
                parentContext.ActivityContext);

            var ok = await handler(ea.Body, ct);
            if (ok) await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false, ct);
            else await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, ct);
        };

        // Use manual ack since handler returns true or false
        await _channel!.BasicConsumeAsync(_queue!, autoAck: false, consumer, ct);
        await Task.Delay(Timeout.Infinite, ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
    }
}