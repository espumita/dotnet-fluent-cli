namespace Fluent.Cli;

public class OptionIsNotConfiguredException : Exception
{
    public OptionIsNotConfiguredException(string message) : base(message) { }
}