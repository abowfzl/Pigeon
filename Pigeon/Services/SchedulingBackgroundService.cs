using Pigeon.Contracts;

namespace Pigeon.Services;

public class SchedulingBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<SchedulingBackgroundService> _logger;

    public SchedulingBackgroundService(IServiceScopeFactory serviceScopeFactory,
        ILogger<SchedulingBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var crawler = scope.ServiceProvider.GetRequiredService<ICrawler>();

                await crawler.Crawl(stoppingToken);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception in SchedulingBackgroundService when ExecutingAsync. {exceptionMessage}", exception.Message);
        }
    }
}