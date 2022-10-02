using Kitchen.Kitchen;

namespace Kitchen.BackgroundTask;

public class BackgroundTask : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BackgroundTask(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(5000, stoppingToken);
            using var scope = _serviceScopeFactory.CreateScope();
            var diningHall = scope.ServiceProvider.GetRequiredService<IKitchen>();
            await diningHall.InitializeKitchenParallelAsync();
            await diningHall.MaintainKitchen();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}