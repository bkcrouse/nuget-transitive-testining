# NuGet Transitive Dependency Testing Repository

[![Build and Publish NuGet Packages](https://github.com/bkcrouse/nuget-transitive-testining/actions/workflows/build-and-publish.yml/badge.svg)](https://github.com/bkcrouse/nuget-transitive-testining/actions/workflows/build-and-publish.yml)

## Overview

This repository demonstrates common NuGet transitive dependency issues and best practices for solving them. It's a hands-on learning resource that showcases real-world dependency management challenges in a C# mono repo architecture.

## 🎯 Purpose

1. **Educate** - Understand NuGet dependency resolution from first principles
2. **Demonstrate** - See common dependency issues in working code
3. **Practice** - Experiment with different resolution strategies
4. **Reference** - Use as a template for proper package management

## 📚 What's Included

### Documentation
- **[NuGet Transitive Dependencies Guide](docs/NUGET_TRANSITIVE_DEPENDENCIES.md)** - Comprehensive breakdown of transitive dependency issues, best practices, and solutions
- **[Expert Prompt](docs/EXPERT_PROMPT.md)** - AI prompt engineering resource for NuGet dependency assistance

### Demo Projects
- **NuGetDemo.Core** - Base library with shared types and interfaces
- **NuGetDemo.Utilities** - Mid-level library depending on Core
- **NuGetDemo.Services** - Service layer depending on Core and Utilities
- **NuGetDemo.ConsumerApp** - Console application demonstrating transitive dependencies

### CI/CD
- **GitHub Actions Workflow** - Automated build, pack, and publish to GitHub Packages
- **NuGet Package Publishing** - Production-ready package deployment pipeline

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK or later
- Git

### Running the Demo

```bash
# Clone the repository
git clone https://github.com/bkcrouse/nuget-transitive-testining.git
cd nuget-transitive-testining

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the consumer application
dotnet run --project src/ConsumerApp/NuGetDemo.ConsumerApp/NuGetDemo.ConsumerApp.csproj

# Inspect dependency graph
dotnet list package --include-transitive
```

### Expected Output

```
=== NuGet Transitive Dependency Demo ===

NuGetDemo.Core v1.0.0
NuGetDemo.Utilities v1.0.0
NuGetDemo.Services v1.0.0

=== Processing Data ===
[INFO] Processing data in DataService
[INFO] Converting object to JSON
...
```

## 🔍 What's Being Demonstrated

### 1. Transitive Dependencies
The `ConsumerApp` only references `Services`, but automatically gets access to `Utilities` and `Core` through transitive dependencies.

```
ConsumerApp
  └─> Services (direct)
       ├─> Utilities (transitive)
       │    └─> Core (transitive)
       └─> Core (transitive)
```

### 2. Diamond Dependency Problem
Both `Services` and `Utilities` depend on `Core`, creating a diamond dependency pattern.

### 3. Version Conflicts
- `Core` references `Newtonsoft.Json 13.0.1`
- `Services` references `Newtonsoft.Json 13.0.3`
- NuGet resolves to the highest compatible version (13.0.3)

### 4. Package vs Project References
Projects use `ProjectReference` during development, but packages use `PackageReference` when published to NuGet.

## 📦 Project Structure

```
nuget-transitive-testining/
├── src/
│   ├── Core/                           # Base library
│   │   └── NuGetDemo.Core/
│   │       ├── ILogger.cs
│   │       ├── ConsoleLogger.cs
│   │       └── CoreVersion.cs
│   ├── Utilities/                      # Mid-level library
│   │   └── NuGetDemo.Utilities/
│   │       ├── StringHelper.cs
│   │       └── UtilitiesVersion.cs
│   ├── Services/                       # Service layer
│   │   └── NuGetDemo.Services/
│   │       ├── DataService.cs
│   │       └── ServicesVersion.cs
│   └── ConsumerApp/                    # Demo application
│       └── NuGetDemo.ConsumerApp/
│           └── Program.cs
├── docs/
│   ├── NUGET_TRANSITIVE_DEPENDENCIES.md
│   └── EXPERT_PROMPT.md
├── .github/
│   └── workflows/
│       └── build-and-publish.yml
├── NuGetTransitiveDependencies.sln
├── Directory.Build.props
└── .gitignore
```

## 🛠️ Key Features

### Dependency Graph Visualization
```bash
# See all dependencies including transitive ones
dotnet list package --include-transitive

# Check for outdated packages
dotnet list package --outdated

# Check for vulnerable packages
dotnet list package --vulnerable
```

### Package Publishing
The GitHub Actions workflow automatically:
1. Builds all projects
2. Runs the demo application
3. Creates NuGet packages
4. Publishes to GitHub Packages (on push to main/master)

### Version Conflict Resolution
The repository demonstrates how NuGet handles version conflicts:
- **Nearest Wins**: Direct dependencies take precedence
- **Highest Compatible**: When versions are compatible, highest wins
- **Explicit Override**: You can force specific versions

## 📖 Learning Path

1. **Start with the docs**: Read [NUGET_TRANSITIVE_DEPENDENCIES.md](docs/NUGET_TRANSITIVE_DEPENDENCIES.md)
2. **Run the demo**: Execute the consumer app and observe behavior
3. **Inspect dependencies**: Use `dotnet list package --include-transitive`
4. **Experiment**: Try changing version numbers and see what breaks
5. **Implement solutions**: Apply best practices from the documentation
6. **Use the prompt**: Leverage [EXPERT_PROMPT.md](docs/EXPERT_PROMPT.md) for AI assistance

## 🎓 Common Issues Covered

| Issue | Description | Solution Demonstrated |
|-------|-------------|----------------------|
| Transitive Dependencies | Indirect dependencies pulled in automatically | Explicit package references |
| Version Conflicts | Multiple packages requiring different versions | Version alignment, CPM |
| Diamond Dependencies | Multiple paths to same dependency | Proper dependency graph design |
| Implicit Dependencies | Using types without declaring packages | Explicit declarations |
| Package vs Project Refs | Confusion between reference types | Clear documentation and examples |

## 🔐 Security

The workflow includes security best practices:
- Automated vulnerability scanning (can be enabled via `dotnet list package --vulnerable`)
- GitHub Packages with token authentication
- No hardcoded credentials

## 🤝 Contributing

This is an educational repository. Feel free to:
- Open issues for questions or clarifications
- Submit PRs with improvements or additional examples
- Use this as a template for your own dependency demos

## 📝 License

This is an educational demonstration repository. Use freely for learning and reference.

## 🔗 Resources

- [NuGet Documentation](https://docs.microsoft.com/en-us/nuget/)
- [Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [Dependency Resolution](https://learn.microsoft.com/en-us/nuget/concepts/dependency-resolution)
- [Semantic Versioning](https://semver.org/)
- [GitHub Packages for NuGet](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry)

## 💡 Key Takeaways

1. **Understand Your Dependencies**: Always know what you're depending on, directly and transitively
2. **Be Explicit**: Declare packages you use, even if available transitively
3. **Version Management**: Use Central Package Management for consistency
4. **Regular Audits**: Check for outdated and vulnerable packages
5. **Automate**: Use CI/CD to catch dependency issues early

---

**Made with ❤️ for the .NET community**

