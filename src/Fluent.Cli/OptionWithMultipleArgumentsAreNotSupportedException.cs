namespace Fluent.Cli;

public class OptionWithMultipleArgumentsAreNotSupportedException : Exception {
    public OptionWithMultipleArgumentsAreNotSupportedException(string message) : base(message) { }
}