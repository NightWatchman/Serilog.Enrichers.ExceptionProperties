using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;
namespace Serilog.Enrichers.ExceptionProperties;

/// Enriches Serilog log events by adding exception properties
/// when the log event includes an exception.
[ExcludeFromCodeCoverage]
public class ExceptionPropertiesEnricher : ILogEventEnricher
{
  public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) =>
    ExceptionEnricher.Enrich(
      logEvent.Exception,
      (name, value) =>
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(name, value)));
}


public static class ExceptionProperties
{
  public static LoggerConfiguration WithExceptionProperties(this LoggerEnrichmentConfiguration @this) =>
    @this == null
      ? throw new ArgumentNullException(nameof(LoggerEnrichmentConfiguration))
      : @this.With(new ExceptionPropertiesEnricher());
}


internal static class ExceptionEnricher
{
  internal delegate void AddProperty(string name, string value);
  
  public static void Enrich(Exception? ex, AddProperty add)
  {
    if (ex is not null)
    {
      add("Exception", ex.Message);
      if (ex.StackTrace is not null)
        add("Stacktrace", ex.StackTrace);
      if (ex.InnerException is not null)
        add("InnerException", ex.InnerException.Message);
    }
  }
}
