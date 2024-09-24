using CoCService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Controller On Connect";
});

builder.Services.AddHostedService<ControllerPollingService>();

var host = builder.Build();
host.Run();