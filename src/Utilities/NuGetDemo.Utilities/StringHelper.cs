using NuGetDemo.Core;
using Newtonsoft.Json;

namespace NuGetDemo.Utilities;

/// <summary>
/// String utilities that depend on Core library
/// This demonstrates transitive dependencies
/// </summary>
public class StringHelper
{
    private readonly ILogger _logger;

    public StringHelper(ILogger logger)
    {
        _logger = logger;
    }

    public string ToJson(object obj)
    {
        _logger.Log("Converting object to JSON");
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public T? FromJson<T>(string json)
    {
        _logger.Log("Converting JSON to object");
        return JsonConvert.DeserializeObject<T>(json);
    }

    public string FormatMessage(string message)
    {
        return $"[Utilities] {message}";
    }
}
