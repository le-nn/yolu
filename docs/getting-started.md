# Yolu

Yolu provides utilities and language extensions aimed to improve development productivity
and extend the .NET API with unique features. 

## Documentation

[https://le-nn.github.io/yolu/](https://le-nn.github.io/yolu/)

## Install

Please install via package manager.

### [.NET CLI](#tab/tabid-1)

```bash
dotnet add package Yolu --version VERSION_NUMBER
```

### [PackageReference](#tab/tabid-2)

```xml
<PackageReference Include="Yolu" Version="VERSION_NUMBER" />
```

### Install from Nuget

[https://www.nuget.org/packages/yolu](https://www.nuget.org/packages/yolu)

## Features

Introducing some of the functions

- **[Range Expression Foreach]([#result](https://le-nn.github.io/yolu/api/Yolu.Foreach.html))**: Make range expression available in foreach.

- Arrays: Provides a safe, fast, convenient and unified array/mutable-array
  
- Buffer: Provides utils and a mechanism to safely and quickly pool Buffers containing native memory

- Error/TryCatch : It supplements features that are not sufficient with system exceptions, and provides a mechanism to handle them safely and type-safely.

- Pipeline: Express functional programming-like pipeline operators with method chains

- Utils: Provides various utils, helpers and extensions
