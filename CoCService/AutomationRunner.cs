using System.Diagnostics;

namespace CoCService;

public class AutomationRunner
{
    private string ScriptLocation { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ControllerOnConnect", "scripts");

    private readonly List<string> _scriptsToRun;
    private readonly ILogger<AutomationRunner> _logger;

    public AutomationRunner(ILogger<AutomationRunner> logger)
    {
        _logger = logger;
        if (!Directory.Exists(ScriptLocation))
        {
            Directory.CreateDirectory(ScriptLocation);
        }
        
        _logger.LogInformation(string.Format("Script location is {0}", ScriptLocation));

        _scriptsToRun = Directory.GetFiles(ScriptLocation).ToList();
    }

    public void Trigger()
    {
        foreach (var script in _scriptsToRun)
        {
            _logger.LogInformation(string.Format("Running ${0}", script));
            RunPowerShellScript(script);
        }
    }
    
    static void RunPowerShellScript(string scriptPath)
    {
        try
        {
            // Create a new process to run PowerShell
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Start the process
            using Process? process = Process.Start(startInfo);
            if (process is null) return;
                
            // Capture the output and errors
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            process.WaitForExit();

            // Display the output and errors
            Console.WriteLine($"Output for {scriptPath}:\n{output}");
            if (!string.IsNullOrEmpty(errors))
            {
                Console.WriteLine($"Errors for {scriptPath}:\n{errors}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running script {scriptPath}: {ex.Message}");
        }
    }
}