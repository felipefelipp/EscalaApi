using EscalaApi.Repositories;

namespace EscalaApi.Services;

public class PreviewCleanupHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public PreviewCleanupHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var previewRepo = scope.ServiceProvider.GetRequiredService<PreviewRepository>();
                await previewRepo.LimparExpiradosAsync();
            }
            catch
            {
                // Ignora falhas de limpeza em ambiente sem banco inicializado
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
