namespace Fluent.Cli.Exceptions;

public class OptionIsNotConfiguredException : Exception
{
    public OptionIsNotConfiguredException(string message) : base(message) { }
}