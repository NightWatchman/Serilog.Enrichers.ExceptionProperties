using FluentAssertions;
using FluentAssertions.Execution;
using Serilog.Enrichers.ExceptionProperties;
using System;
using Xunit;
namespace Tests.ExceptionPropertiesEnricherTests;

public class Enrich
{
  [Fact]
  public void EventHasNoException_IsNoOp()
  {
    ExceptionEnricher.Enrich(null, (_, _) =>
      throw new AssertionFailedException("Tried to enrich with no exception"));
  }

  [Fact]
  public void EventHasException_AddsExceptionMessageProperty()
  {
    var msg = "Test";
    ExpectToHaveBeenCalledWith("Exception", msg, add =>
    {
      ExceptionEnricher.Enrich(new(msg), add);
    });
  }

  [Fact]
  public void EventExceptionHasNoStacktrace_DoesNotAddStacktrace()
  {
    ExpectNotToHaveBeenCalledWith("Stacktrace", add =>
    {
      ExceptionEnricher.Enrich(new("Test"), add);
    });
  }

  [Fact]
  public void EventHasExceptionWithStacktrace_AddsExceptionStacktraceProperty()
  {
    try
    {
      throw new("Test");
    }
    catch (Exception ex)
    {
      ExpectToHaveBeenCalledWith("Stacktrace", add =>
      {
        ExceptionEnricher.Enrich(ex, add);
      });
    }
  }

  [Fact]
  public void EventHasInnerException_AddsInnerExceptionMessageProperty()
  {
    var msg = "Innie";
    ExpectToHaveBeenCalledWith("InnerException", "Innie", add =>
    {
      ExceptionEnricher.Enrich(new("Outie", new(msg)), add);
    });
  }

  // TODO: Generalize and export to fluentassertions extension library
  private void ExpectToHaveBeenCalledWith(string name, string value, Action<ExceptionEnricher.AddProperty> act)
  {
    var result = false;
    ExceptionEnricher.AddProperty add = (addName, addValue) =>
    {
      if (addName == name && addValue == value)
        result = true;
    };

    act(add);

    result.Should().BeTrue();
  }

  private void ExpectToHaveBeenCalledWith(string name, Action<ExceptionEnricher.AddProperty> act)
  {
    var result = false;
    ExceptionEnricher.AddProperty add = (addName, addValue) =>
    {
      if (addName == name && !String.IsNullOrEmpty(addValue))
        result = true;
    };

    act(add);

    result.Should().BeTrue();
  }

  private void ExpectNotToHaveBeenCalledWith(string name, Action<ExceptionEnricher.AddProperty> act)
  {
    var result = true;
    ExceptionEnricher.AddProperty add = (addName, _) =>
    {
      if (addName == name)
        result = false;
    };

    act(add);

    result.Should().BeTrue();
  }
}
