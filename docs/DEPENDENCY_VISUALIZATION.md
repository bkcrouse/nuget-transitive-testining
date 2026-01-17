# Dependency Visualization

## Project Dependency Graph

```
┌─────────────────────────────────────┐
│         ConsumerApp                  │
│    (Console Application)             │
│                                      │
│    Only references: Services         │
│                                      │
│    Gets transitively:                │
│    - Core                            │
│    - Utilities                       │
│    - Newtonsoft.Json 13.0.3          │
└───────────────┬─────────────────────┘
                │
                │ ProjectReference
                │
                ▼
┌─────────────────────────────────────┐
│           Services                   │
│       (Class Library)                │
│                                      │
│    References:                       │
│    - Core (ProjectReference)         │
│    - Utilities (ProjectReference)    │
│    - Newtonsoft.Json 13.0.3 ⚠️      │
└─────────┬──────────────┬────────────┘
          │              │
          │              │ ProjectReference
          │              │
          │              ▼
          │     ┌─────────────────────────────┐
          │     │       Utilities              │
          │     │    (Class Library)           │
          │     │                              │
          │     │    References:               │
          │     │    - Core (ProjectReference) │
          │     │                              │
          │     │    Gets transitively:        │
          │     │    - Newtonsoft.Json 13.0.1  │
          │     └──────────┬──────────────────┘
          │                │
          │                │ ProjectReference
          │                │
          ▼                ▼
┌─────────────────────────────────────┐
│             Core                     │
│       (Class Library)                │
│                                      │
│    References:                       │
│    - Newtonsoft.Json 13.0.1 ⚠️      │
└─────────────────────────────────────┘
```

## Issue: Version Conflict

**Core** wants: `Newtonsoft.Json 13.0.1`
**Services** wants: `Newtonsoft.Json 13.0.3`

⚠️ **This creates a version conflict!**

## Resolution: NuGet's Strategy

NuGet uses "highest compatible version wins":

1. **Core** → Newtonsoft.Json 13.0.1
2. **Utilities** → Gets 13.0.1 transitively from Core
3. **Services** → Newtonsoft.Json 13.0.3 (higher version)
4. **ConsumerApp** → Gets 13.0.3 from Services (highest wins)

**Result**: All projects end up using 13.0.3

## Diamond Dependency Pattern

```
       ConsumerApp
           │
           ├──► Services ──► Core
           │        │
           │        └──► Utilities ──► Core
           │
           └──► (transitively) Utilities ──► Core
```

**Core** is reached through two paths:
1. ConsumerApp → Services → Core
2. ConsumerApp → Services → Utilities → Core

This is called a "diamond dependency" because of its shape.

## Package Flow (Simplified)

```
┌──────────────┐
│ Newtonsoft   │
│ Json 13.0.1  │──┐
└──────────────┘  │
                  ├──► Core ◄────┬──────────────┐
┌──────────────┐  │              │              │
│ Newtonsoft   │  │         Utilities      Services
│ Json 13.0.3  │──┘              │              │
└──────────────┘                 │              │
                                 └──────┬───────┘
                                        │
                                  ConsumerApp
```

## Transitive Dependency Chain

```
ConsumerApp.csproj
  <ProjectReference Include="Services" />
      ↓ brings in transitively
      Services.csproj
        <ProjectReference Include="Core" />
        <ProjectReference Include="Utilities" />
        <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
            ↓ brings in transitively
            Utilities.csproj
              <ProjectReference Include="Core" />
                  ↓ brings in transitively
                  Core.csproj
                    <PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
```

## What ConsumerApp Actually Gets

Even though ConsumerApp only references Services:

```xml
<!-- ConsumerApp.csproj -->
<ItemGroup>
  <ProjectReference Include="../../Services/NuGetDemo.Services/NuGetDemo.Services.csproj" />
</ItemGroup>
```

At runtime, it has access to:
- ✅ Services.dll
- ✅ Utilities.dll (transitive)
- ✅ Core.dll (transitive)
- ✅ Newtonsoft.Json.dll v13.0.3 (transitive, highest version)

## Command to Verify

```bash
dotnet list package --include-transitive
```

Output shows:
```
Project 'NuGetDemo.Core' has the following package references
   [net8.0]: 
   Top-level Package      Requested   Resolved
   > Newtonsoft.Json      13.0.1      13.0.1  

Project 'NuGetDemo.Utilities' has the following package references
   [net8.0]: 
   Transitive Package      Resolved
   > Newtonsoft.Json       13.0.1  

Project 'NuGetDemo.Services' has the following package references
   [net8.0]: 
   Top-level Package      Requested   Resolved
   > Newtonsoft.Json      13.0.3      13.0.3  

Project 'NuGetDemo.ConsumerApp' has the following package references
   [net8.0]: 
   Transitive Package      Resolved
   > Newtonsoft.Json       13.0.3
```

## Key Observations

1. **Transitive Dependencies Work**: ConsumerApp only references Services but gets everything
2. **Version Resolution Works**: Despite conflict, NuGet picks highest compatible version
3. **Diamond Pattern Handled**: Core is reached via two paths, but only included once
4. **Type Safety Maintained**: All projects can compile and run with resolved versions

## Alternative Scenarios

### Scenario A: Explicit Version Override

If ConsumerApp explicitly requests 13.0.1:
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
```
Result: 13.0.1 wins (nearest wins principle)

### Scenario B: Incompatible Versions

If Core wanted 12.x and Services wanted 13.x:
- NuGet would pick the highest (13.x)
- Risk: Breaking changes between major versions might cause runtime errors

### Scenario C: Central Package Management

With Directory.Packages.props:
```xml
<PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
```
Result: All projects use 13.0.3, no conflicts

## Best Practice

**Always be explicit about direct dependencies:**

```xml
<!-- If you use Newtonsoft.Json directly in your code -->
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

Don't rely on transitive dependencies for packages you directly use!
