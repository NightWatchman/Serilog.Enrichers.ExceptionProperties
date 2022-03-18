# Serilog.Enrichers.ExceptionProperties

Enriches Serilog events with `Exception` properties when `Exception`s are logged. The included properties are `Message`, `Stacktrace`, and `InnerException.Message`. They are enhanced as `Exception`, `Stacktrace`, and `InnerException` properties respectively.

To use the enricher, first install the NuGet package:

```powershell
Install-Package Serilog.Enrichers.ExceptionProperties
```

Then, apply the enricher to your `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
  .Enrich.WithExceptionProperties()
  // ...other configuration...
  .CreateLogger();
```

Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html).
