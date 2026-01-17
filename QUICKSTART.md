# Quick Start Guide

Get up and running with the NuGet Transitive Dependency demonstration in 5 minutes.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Git
- A terminal/command prompt

## Step 1: Clone the Repository

```bash
git clone https://github.com/bkcrouse/nuget-transitive-testining.git
cd nuget-transitive-testining
```

## Step 2: Build the Solution

```bash
dotnet restore
dotnet build
```

Expected output: `Build succeeded. 0 Warning(s) 0 Error(s)`

## Step 3: Run the Demo

```bash
dotnet run --project src/ConsumerApp/NuGetDemo.ConsumerApp/NuGetDemo.ConsumerApp.csproj
```

You should see output showing:
- Version information for all three libraries
- Data processing with JSON serialization
- Logging output from the Core library

## Step 4: Inspect Dependencies

```bash
dotnet list package --include-transitive
```

This shows:
- **Core**: Requests Newtonsoft.Json 13.0.1
- **Utilities**: Gets Newtonsoft.Json 13.0.1 transitively from Core
- **Services**: Requests Newtonsoft.Json 13.0.3 (version conflict!)
- **ConsumerApp**: Gets Newtonsoft.Json 13.0.3 (highest version wins)

## Step 5: Create NuGet Packages

```bash
dotnet pack --configuration Release --output ./packages
```

This creates three `.nupkg` files in the `packages/` directory.

## What's Next?

### Learn the Concepts
Read [docs/NUGET_TRANSITIVE_DEPENDENCIES.md](docs/NUGET_TRANSITIVE_DEPENDENCIES.md) for a comprehensive explanation of:
- What are transitive dependencies
- Common issues (diamond dependencies, version conflicts)
- Best practices and solutions
- First principles breakdown

### Experiment
Try these experiments to learn more:

**Experiment 1: Change a Version**
1. Edit `src/Core/NuGetDemo.Core/NuGetDemo.Core.csproj`
2. Change Newtonsoft.Json version from 13.0.1 to 12.0.0
3. Run `dotnet build`
4. Observe how NuGet resolves the conflict

**Experiment 2: Add Explicit Reference**
1. Edit `src/ConsumerApp/NuGetDemo.ConsumerApp/NuGetDemo.ConsumerApp.csproj`
2. Add: `<PackageReference Include="Newtonsoft.Json" Version="13.0.1" />`
3. Run `dotnet list package --include-transitive`
4. See how the explicit reference affects resolution

**Experiment 3: Enable Central Package Management**
1. Rename `Directory.Packages.props.example` to `Directory.Packages.props`
2. Update `Directory.Build.props` to set `ManagePackageVersionsCentrally` to `true`
3. Remove `Version` attributes from all `PackageReference` elements
4. Run `dotnet restore && dotnet build`

### Explore Commands
Check out [docs/COMMANDS.md](docs/COMMANDS.md) for:
- Essential dotnet commands
- Diagnostic tools
- Troubleshooting tips
- CI/CD integration examples

### Use the Expert Prompt
If you're using an AI assistant (like GitHub Copilot or ChatGPT), use the prompt in [docs/EXPERT_PROMPT.md](docs/EXPERT_PROMPT.md) to get expert-level NuGet dependency assistance.

## Common Issues

### "dotnet: command not found"
Install the .NET SDK from https://dotnet.microsoft.com/download

### Build fails with "Could not find a part of the path"
Make sure you're in the repository root directory (where `NuGetTransitiveDependencies.sln` is located).

### Package restore fails
Check your internet connection and NuGet sources:
```bash
dotnet nuget list source
```

## Project Structure at a Glance

```
ConsumerApp (references Services only)
    ↓
Services (references Core + Utilities + Newtonsoft.Json 13.0.3)
    ↓
Utilities (references Core)
    ↓
Core (references Newtonsoft.Json 13.0.1)
```

**Key Point**: ConsumerApp only explicitly references Services, but gets Utilities and Core through transitive dependencies.

## Key Files to Explore

| File | Purpose |
|------|---------|
| `README.md` | Main documentation |
| `docs/NUGET_TRANSITIVE_DEPENDENCIES.md` | Comprehensive guide |
| `docs/EXPERT_PROMPT.md` | AI prompt for dependency help |
| `docs/COMMANDS.md` | Command reference |
| `.github/workflows/build-and-publish.yml` | CI/CD pipeline |
| `nuget.config` | NuGet configuration |
| `Directory.Build.props` | Shared build properties |

## Learning Path

1. ✅ **Quick Start** (you are here) - 5 minutes
2. 📖 **Read README.md** - 10 minutes
3. 🔍 **Explore the code** - 15 minutes
4. 📚 **Deep dive into docs/** - 30-60 minutes
5. 🧪 **Experiment with changes** - As long as you want!

## Getting Help

- Read the comprehensive documentation in `docs/`
- Use the expert prompt in `docs/EXPERT_PROMPT.md` with AI assistants
- Check common commands in `docs/COMMANDS.md`
- Open an issue on GitHub for questions

## Summary

You now have a working demonstration of NuGet transitive dependencies! The repository includes:
- ✅ 4 interconnected C# projects
- ✅ Working demo application
- ✅ Version conflict demonstration
- ✅ Diamond dependency pattern
- ✅ CI/CD pipeline for GitHub Actions
- ✅ Comprehensive documentation

**Next step**: Read [docs/NUGET_TRANSITIVE_DEPENDENCIES.md](docs/NUGET_TRANSITIVE_DEPENDENCIES.md) to understand the why behind what you just saw!
