using Microsoft.Extensions.Options;
using MyFeedbackHub.SharedKernel.Configurations;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MyFeedbackHub.Infrastructure.AMQP;

public class MessagePublisherServicer : IMessagePublisherService, IAsyncDisposable
{
    private IConnection? _connection;
    private readonly IChannel? _channel;
    private readonly RabbitMQConfigurations _options;

    public MessagePublisherServicer(IOptions<RabbitMQConfigurations> options)
    {
        _options = options.Value;

        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.Username,
            Password = _options.Password
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        _channel.QueueDeclareAsync(_options.QueueName, durable: true, exclusive: false, autoDelete: false)
                .Wait();
    }

    public async Task PublishAsync<T>(T message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel!.BasicPublishAsync(
            exchange: "",
            routingKey: _options.QueueName,
            body: body);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}

public interface IMessagePublisherService
{
    Task PublishAsync<T>(T message);
}

public record UserCreated
{
    public Guid UserId { get; set; }
}