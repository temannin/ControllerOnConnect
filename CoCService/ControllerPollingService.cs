
using System.Timers;

namespace CoCService;

public sealed class ControllerPollingService : BackgroundService
{
    private readonly System.Timers.Timer _timer;
    private readonly ILogger<ControllerPollingService> _logger;
    private readonly ControllerFinder _finder;
    private readonly AutomationRunner _runner;

    public ControllerPollingService(
        ControllerFinder finder,
        AutomationRunner runner,
        ILogger<ControllerPollingService> logger)
    {
        _logger = logger;
        _finder = finder;
        _runner = runner;
        _timer = new System.Timers.Timer(1000) { AutoReset = true };
        _timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            if (_finder.ControllerIsConnected)
            {
                _logger.LogInformation("Controller is connected. Running automations.");
                _runner.Trigger();
            }
            else
            {
                _logger.LogDebug("No controller is connected");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in timer execution: {Message}", ex.Message);

            // Terminates this process to ensure Windows Service recovery options take place
            Environment.Exit(1);
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer.Start();
        return Task.CompletedTask;
    }
    
    
}