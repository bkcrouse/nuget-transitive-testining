namespace NuGetDemo.Core;

/// <summary>
/// Version information for Core library - helps demonstrate version conflicts
/// </summary>
public static class CoreVersion
{
    public const string Version = "1.0.0";
    
    public static string GetVersionInfo()
    {
        return $"NuGetDemo.Core v{Version}";
    }
}
