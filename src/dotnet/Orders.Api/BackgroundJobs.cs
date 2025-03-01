﻿using Orders.Application.Interfaces;

namespace Orders.Api;

public class BackgroundJobs : BackgroundService
{
    private readonly ILogger<BackgroundJobs> _logger;
    private readonly IServiceProvider _services;
    private readonly List<Timer> _timers = new List<Timer>();

    public BackgroundJobs(ILogger<BackgroundJobs> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Only start the timer if the app is running in the Production environment
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
            return Task.CompletedTask;

        _logger.LogInformation("Background jobs started");

        // Starts a timer that fires after 15 minutes and then every 1 hour after that
        _timers.Add(new Timer(CheckShippingStatus, null, TimeSpan.FromMinutes(15), TimeSpan.FromHours(1)));

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping jobs...");
        _timers.ForEach(x => x?.Change(Timeout.Infinite, 0));
        return base.StopAsync(cancellationToken);
    }

    // Add jobs logic here
    private async void CheckShippingStatus(object? state)
    {
        _logger.LogInformation("Checking shipping status for orders...");

        using var scope = _services.CreateScope();
        var shippingStatusChecker = scope.ServiceProvider.GetRequiredService<IShippingStatusJob>();
        await shippingStatusChecker.CheckStatus();

        _logger.LogInformation("Finished checking shipping status for orders");
    }
}