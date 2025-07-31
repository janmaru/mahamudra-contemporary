# Mahamudra Contemporary

![Contemporary Banner](paul-engel-bx4JS1lGB0U-unsplash.jpg)
*Photo by Paul Engel on Unsplash*

[![NuGet Package](https://img.shields.io/nuget/v/Mahamudra.Contemporary.svg)](https://www.nuget.org/packages/Mahamudra.Contemporary/)
[![.NET Standard 2.1](https://img.shields.io/badge/.NET%20Standard-2.1-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

A robust Task Parallel Library that combines **Railway Oriented Programming** with parallel execution patterns. Execute collections of tasks concurrently while maintaining functional programming principles and comprehensive error handling.

## ✨ Features

- **Parallel Execution**: Execute functions concurrently across collections with configurable parallelism
- **Railway Oriented Programming**: Built-in `Result<T, E>` pattern for elegant error handling
- **Comprehensive Logging**: Integrated Microsoft.Extensions.Logging support with detailed execution tracking
- **Performance Optimized**: Async-first design with efficient memory usage
- **Thread-Safe**: Built on `ConcurrentDictionary` for safe parallel operations
- **.NET Standard 2.1**: Compatible with .NET Core 3.1+, .NET 5+, and .NET Framework 4.6.1+

## 🚀 Quick Start

### Installation

```bash
dotnet add package Mahamudra.Contemporary
```

### Basic Usage

```csharp
using Mahamudra.Contemporary;
using Microsoft.Extensions.Logging;

// Create factory with optional logging
var factory = new ParallelAsyncFactory(logger);

// Execute async operations in parallel
var results = await factory.ExecuteAsyncResult<int, string>(
    numbers, 
    async number => await ProcessNumberAsync(number)
);

// Handle results using Railway Oriented Programming
var successes = results.Where(r => r.Key is Success<int, string>);
var failures = results.Where(r => r.Key is Failure<int, string>);
```

## 📚 API Reference

### ParallelAsyncFactory

The main class providing parallel execution capabilities with Railway Oriented Programming patterns.

#### Methods

```csharp
// Execute async function with Result pattern
Task<ConcurrentDictionary<Result<T, string>, M>> ExecuteAsyncResult<T, M>(
    IEnumerable<T> list, 
    Func<T, Task<M>> function
)
```

**Parameters:**
- `list`: Collection of items to process
- `function`: Async function to execute for each item

**Returns:** `ConcurrentDictionary` where:
- **Key**: `Result<T, string>` - Either `Success<T>` containing the original item, or `Failure<string>` with error details
- **Value**: `M` - The result of the function execution (or default for failures)

### Constructors

```csharp
// Default constructor (uses NullLogger)
ParallelAsyncFactory()

// Constructor with custom logger
ParallelAsyncFactory(ILogger logger)
```

## 💡 Examples

### Processing a List of URLs

```csharp
var urls = new[] { "https://api1.com", "https://api2.com", "https://api3.com" };
var factory = new ParallelAsyncFactory(logger);

var results = await factory.ExecuteAsyncResult<string, HttpResponseMessage>(
    urls,
    async url => await httpClient.GetAsync(url)
);

// Process successful responses
foreach (var success in results.Where(r => r.Key is Success<string, string>))
{
    var originalUrl = ((Success<string, string>)success.Key).Value;
    var response = success.Value;
    Console.WriteLine($"✅ {originalUrl}: {response.StatusCode}");
}

// Handle failures
foreach (var failure in results.Where(r => r.Key is Failure<string, string>))
{
    var error = ((Failure<string, string>)failure.Key).Error;
    Console.WriteLine($"❌ Error: {error}");
}
```

### Data Processing Pipeline

```csharp
var dataIds = Enumerable.Range(1, 100);
var processor = new ParallelAsyncFactory(logger);

var processedData = await processor.ExecuteAsyncResult<int, ProcessedResult>(
    dataIds,
    async id => await ProcessDataAsync(id)
);

var successCount = processedData.Count(r => r.Key is Success<int, string>);
var errorCount = processedData.Count(r => r.Key is Failure<int, string>);

Console.WriteLine($"Processed: {successCount} successful, {errorCount} failed");
```

## 🔧 Configuration

### Logging

The library integrates with `Microsoft.Extensions.Logging`. Configure your logger as usual:

```csharp
// In Startup.cs or Program.cs
services.AddLogging(builder => 
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});

// Inject into your service
public class MyService
{
    public MyService(ILogger<MyService> logger)
    {
        _factory = new ParallelAsyncFactory(logger);
    }
}
```

## 📊 Performance

Performance comparison between synchronous parallel execution and async parallel execution:

| Method | Mean | Error | StdDev | Allocated |
|--------|------|--------|--------|-----------|
| ExecuteParallelResult | 731.6 ms | 15,086.4 ms | 826.9 ms | 307.29 KB |
| **ExecuteAsyncResult** | **206.5 ms** | **243.0 ms** | **13.32 ms** | **270.69 KB** |

*Async execution shows ~3.5x better performance with lower memory allocation.*

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

Example test demonstrating error handling:

```csharp
[TestMethod]
public async Task ExecuteAsyncResult_HandlesPartialFailures()
{
    var items = new[] { 1, 2, 3, 4, 5 };
    var results = await factory.ExecuteAsyncResult<int, int>(
        items, 
        async x => x == 3 ? throw new Exception("Test error") : x * 2
    );
    
    var successes = results.Where(r => r.Key is Success<int, string>);
    var failures = results.Where(r => r.Key is Failure<int, string>);
    
    Assert.AreEqual(4, successes.Count());
    Assert.AreEqual(1, failures.Count());
    Assert.AreEqual(16, successes.Sum(s => s.Value)); // 2+4+8+10 = 24, but 3 failed
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Run the test suite
6. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Railway Oriented Programming pattern by Scott Wlaschin
- Lotus flower icon by Freepik - Flaticon
- Photo by Paul Engel on Unsplash

---

**Repository**: [https://github.com/janmaru/mahamudra-contemporary](https://github.com/janmaru/mahamudra-contemporary)  
**NuGet Package**: [Mahamudra.Contemporary](https://www.nuget.org/packages/Mahamudra.Contemporary/)
  