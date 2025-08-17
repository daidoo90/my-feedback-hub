using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyFeedbackHub.BackgroundServices.Services;

public class OutboxProcessor : BackgroundService
{
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(ILogger<OutboxProcessor> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MyBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Doing background work at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }

        _logger.LogInformation("MyBackgroundService is stopping.");
    }
}