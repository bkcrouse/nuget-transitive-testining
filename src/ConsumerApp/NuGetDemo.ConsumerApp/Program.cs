using NuGetDemo.Core;
using NuGetDemo.Utilities;
using NuGetDemo.Services;

namespace NuGetDemo.ConsumerApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== NuGet Transitive Dependency Demo ===\n");

        // Display version information
        Console.WriteLine(CoreVersion.GetVersionInfo());
        Console.WriteLine(UtilitiesVersion.GetVersionInfo());
        Console.WriteLine(ServicesVersion.GetVersionInfo());
        Console.WriteLine();

        // Create instances
        var logger = new ConsoleLogger();
        var stringHelper = new StringHelper(logger);
        var dataService = new DataService(logger, stringHelper);

        // Demonstrate functionality
        var sampleData = new Dictionary<string, object>
        {
            { "name", "NuGet Demo" },
            { "version", "1.0.0" },
            { "demonstrates", new[] { "Transitive Dependencies", "Diamond Dependency", "Version Conflicts" } }
        };

        Console.WriteLine("\n=== Processing Data ===");
        var json = dataService.ProcessData(sampleData);
        
        Console.WriteLine("\n=== Parsing Data ===");
        var parsedData = dataService.ParseData(json);
        
        if (parsedData != null)
        {
            logger.Log($"Successfully parsed {parsedData.Count} items");
        }

        Console.WriteLine("\n=== Demo Complete ===");
    }
}

