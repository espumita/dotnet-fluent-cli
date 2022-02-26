namespace Fluent.Cli.Exceptions;

public class OptionWithMultipleArgumentsAreNotSupportedException : Exception {
    public OptionWithMultipleArgumentsAreNotSupportedException(string message) : base(message) { }
}