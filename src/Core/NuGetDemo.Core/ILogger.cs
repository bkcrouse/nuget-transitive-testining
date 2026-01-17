namespace NuGetDemo.Core;

/// <summary>
/// Core logging interface - demonstrates base dependency
/// </summary>
public interface ILogger
{
    void Log(string message);
    void LogError(string message);
    void LogWarning(string message);
}
