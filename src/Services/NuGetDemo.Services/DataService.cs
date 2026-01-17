using NuGetDemo.Core;
using NuGetDemo.Utilities;
using Newtonsoft.Json;

namespace NuGetDemo.Services;

/// <summary>
/// Data service that depends on both Core and Utilities
/// This demonstrates the diamond dependency problem
/// </summary>
public class DataService
{
    private readonly ILogger _logger;
    private readonly StringHelper _stringHelper;

    public DataService(ILogger logger, StringHelper stringHelper)
    {
        _logger = logger;
        _stringHelper = stringHelper;
    }

    public string ProcessData(Dictionary<string, object> data)
    {
        _logger.Log("Processing data in DataService");
        
        var json = _stringHelper.ToJson(data);
        _logger.Log($"Data serialized: {json}");
        
        return json;
    }

    public Dictionary<string, object>? ParseData(string json)
    {
        _logger.Log("Parsing data in DataService");
        return _stringHelper.FromJson<Dictionary<string, object>>(json);
    }
}
