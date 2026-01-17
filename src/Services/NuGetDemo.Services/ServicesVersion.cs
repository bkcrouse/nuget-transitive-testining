namespace NuGetDemo.Services;

/// <summary>
/// Version information for Services library
/// </summary>
public static class ServicesVersion
{
    public const string Version = "1.0.0";
    
    public static string GetVersionInfo()
    {
        return $"NuGetDemo.Services v{Version}";
    }
}
