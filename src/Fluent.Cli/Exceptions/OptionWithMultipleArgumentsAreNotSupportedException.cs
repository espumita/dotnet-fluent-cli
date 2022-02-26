namespace Fluent.Cli.Exceptions;

public class OptionWithMultipleArgumentsAreNotSupportedException : CliArgumentsBuilderConfigurationException {
    public OptionWithMultipleArgumentsAreNotSupportedException(string message) : base(message) { }
}