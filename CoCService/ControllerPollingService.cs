
using System.Timers;
using SharpDX.DirectInput;

namespace CoCService;

public sealed class ControllerPollingService : BackgroundService
{
    private readonly System.Timers.Timer _timer;
    private readonly ILogger<ControllerPollingService> _logger;

    public ControllerPollingService(
        ILogger<ControllerPollingService> logger)
    {
        _logger = logger;
        _timer = new System.Timers.Timer(1000) { AutoReset = true };
        _timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            var directInput = new DirectInput();
            var joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly))
            {
                joystickGuid = deviceInstance.InstanceGuid;
            }

            _logger.LogInformation(joystickGuid == Guid.Empty
                ? "No controller connected"
                : "A controller is connected");
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