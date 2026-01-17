namespace NuGetDemo.Core;

/// <summary>
/// Simple console logger implementation
/// </summary>
public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }

    public void LogError(string message)
    {
        Console.WriteLine($"[ERROR] {message}");
    }

    public void LogWarning(string message)
    {
        Console.WriteLine($"[WARN] {message}");
    }
}
