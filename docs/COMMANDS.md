# Common NuGet Commands and Diagnostics

## Essential Commands

### Package Management

```bash
# Restore all packages
dotnet restore

# Restore with verbose output
dotnet restore --verbosity detailed

# Clean build artifacts
dotnet clean

# Build solution
dotnet build

# Build in Release mode
dotnet build --configuration Release
```

### Dependency Analysis

```bash
# List all packages (direct dependencies only)
dotnet list package

# List all packages including transitive dependencies
dotnet list package --include-transitive

# Check for outdated packages
dotnet list package --outdated

# Check for vulnerable packages
dotnet list package --vulnerable

# Check for deprecated packages
dotnet list package --deprecated
```

### Package Creation

```bash
# Create NuGet package from project
dotnet pack

# Pack with specific version
dotnet pack /p:Version=1.0.0

# Pack in Release configuration with output directory
dotnet pack --configuration Release --output ./packages

# Pack all projects in solution
dotnet pack --configuration Release --output ./packages
```

### Package Publishing

```bash
# Publish to NuGet.org
dotnet nuget push package.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json

# Publish to GitHub Packages
dotnet nuget push package.nupkg --api-key YOUR_GITHUB_TOKEN --source https://nuget.pkg.github.com/OWNER/index.json

# Configure GitHub Packages source
dotnet nuget add source "https://nuget.pkg.github.com/OWNER/index.json" \
  --name github \
  --username YOUR_USERNAME \
  --password YOUR_GITHUB_TOKEN \
  --store-password-in-clear-text
```

## Diagnostic Commands

### Project Analysis

```bash
# Show project info and dependencies
dotnet msbuild /t:Restore,CollectPackageReferences /p:DesignTimeBuild=true

# Generate dependency graph (requires dotnet-outdated tool)
dotnet tool install --global dotnet-outdated-tool
dotnet outdated

# Analyze package references
dotnet restore --force
dotnet msbuild /t:_GenerateRestoreGraph
```

### Version Resolution

```bash
# Force restore to see version resolution
dotnet restore --force --no-cache

# See package resolution with verbosity
dotnet restore --verbosity detailed | grep -A5 "Package 'Newtonsoft.Json'"

# Check actual resolved versions
dotnet msbuild /t:ResolvePackageAssets /p:DesignTimeBuild=true
```

### Troubleshooting

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Verify package sources
dotnet nuget list source

# Add package source
dotnet nuget add source https://api.nuget.org/v3/index.json --name nuget.org

# Remove package source
dotnet nuget remove source NAME

# Check for binding redirects issues (.NET Framework)
# Look in bin/Debug/net48/YourApp.dll.config

# Generate binding redirects automatically (.NET Framework)
# Add to .csproj:
# <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
```

## Advanced Diagnostics

### MSBuild Binary Log

```bash
# Generate detailed build log
dotnet build /bl:build.binlog

# Analyze with MSBuild Structured Log Viewer
# Download from: http://msbuildlog.com/
```

### Package Dependency Tree

```bash
# Install dotnet-tree
dotnet tool install -g dotnet-tree

# Show dependency tree
dotnet tree
```

### NuGet Resolver Analysis

```bash
# Show why a package was included
dotnet msbuild /t:Restore /v:diag > restore.log
grep "Newtonsoft.Json" restore.log
```

## Useful Tools

### Install Essential Tools

```bash
# Package vulnerability checker
dotnet tool install --global dotnet-outdated-tool

# Dependency tree viewer
dotnet tool install --global dotnet-tree

# Format analyzer
dotnet tool install --global dotnet-format
```

## This Repository Specific Commands

### Build and Run

```bash
# Full build from clean state
dotnet clean && dotnet restore && dotnet build

# Run the demo application
dotnet run --project src/ConsumerApp/NuGetDemo.ConsumerApp/NuGetDemo.ConsumerApp.csproj

# Build and run in Release
dotnet build --configuration Release
dotnet run --project src/ConsumerApp/NuGetDemo.ConsumerApp/NuGetDemo.ConsumerApp.csproj --configuration Release --no-build
```

### Create Packages

```bash
# Create all three library packages
dotnet pack src/Core/NuGetDemo.Core/NuGetDemo.Core.csproj --configuration Release --output ./packages
dotnet pack src/Utilities/NuGetDemo.Utilities/NuGetDemo.Utilities.csproj --configuration Release --output ./packages
dotnet pack src/Services/NuGetDemo.Services/NuGetDemo.Services.csproj --configuration Release --output ./packages

# List created packages
ls -la ./packages/*.nupkg
```

### Analyze Dependencies

```bash
# See the version conflict resolution
dotnet list package --include-transitive

# Expected output shows:
# - Core requests Newtonsoft.Json 13.0.1
# - Services requests Newtonsoft.Json 13.0.3
# - ConsumerApp gets 13.0.3 (highest wins)
```

## CI/CD Integration

### GitHub Actions Example

```yaml
# In .github/workflows/build.yml
- name: Restore dependencies
  run: dotnet restore

- name: Check for vulnerable packages
  run: dotnet list package --vulnerable --include-transitive

- name: Build
  run: dotnet build --no-restore --configuration Release

- name: Create packages
  run: dotnet pack --no-build --configuration Release --output ./packages

- name: Publish to GitHub Packages
  run: dotnet nuget push "./packages/*.nupkg" --source github --api-key ${{ secrets.GITHUB_TOKEN }}
```

## Common Issues and Solutions

### Issue: Version Conflict

```bash
# Symptom: Build warning NU1608
# Solution 1: Check what versions are resolved
dotnet list package --include-transitive

# Solution 2: Add explicit version in consuming project
# Edit ConsumerApp.csproj and add:
# <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />

# Solution 3: Use Central Package Management
# Create Directory.Packages.props with centralized versions
```

### Issue: Package Not Found

```bash
# Check package sources
dotnet nuget list source

# Verify package exists in source
# For GitHub Packages, ensure authentication is configured
dotnet nuget add source "https://nuget.pkg.github.com/OWNER/index.json" \
  --name github \
  --username YOUR_USERNAME \
  --password YOUR_TOKEN \
  --store-password-in-clear-text

# Clear cache and retry
dotnet nuget locals all --clear
dotnet restore --force
```

### Issue: Transitive Dependency Missing

```bash
# Symptom: Runtime error "Could not load file or assembly"
# Solution: Add explicit PackageReference
# If you use types from a transitive package, declare it explicitly

# Find which package provides the missing assembly
dotnet list package --include-transitive | grep "MissingAssembly"
```

### Issue: Different Versions in Different Projects

```bash
# Check all projects
for proj in $(find . -name "*.csproj"); do
  echo "=== $proj ==="
  dotnet list "$proj" package
done

# Solution: Implement Central Package Management
# Use Directory.Packages.props to centralize versions
```

## Performance Tips

```bash
# Use parallel restore
dotnet restore --parallel

# Skip expensive checks during CI
dotnet build --no-restore

# Use incremental builds
dotnet build --no-restore --no-dependencies

# Cache NuGet packages in CI
# GitHub Actions example:
# - uses: actions/cache@v3
#   with:
#     path: ~/.nuget/packages
#     key: ${{ runner.os }}-nuget-${{ hashFiles('**/packages.lock.json') }}
```

## Best Practices

1. **Always commit**: `packages.lock.json` for reproducible builds
2. **Use explicit versions**: Avoid wildcards like `*` or floating versions
3. **Regular audits**: Run `dotnet list package --vulnerable` regularly
4. **Update strategy**: Keep dependencies up-to-date with a regular cadence
5. **Document decisions**: Add comments in .csproj explaining version choices
6. **Test updates**: Always test package updates in a separate branch
7. **Monitor deprecations**: Run `dotnet list package --deprecated` periodically

## Additional Resources

- [.NET CLI Reference](https://docs.microsoft.com/en-us/dotnet/core/tools/)
- [NuGet CLI Reference](https://docs.microsoft.com/en-us/nuget/reference/nuget-exe-cli-reference)
- [MSBuild Reference](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-reference)
