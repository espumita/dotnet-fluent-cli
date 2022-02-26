namespace Fluent.Cli.Exceptions;

public class ArgumentIsNotConfiguredException : CliArgumentsBuilderConfigurationException {
    public ArgumentIsNotConfiguredException(string message) : base(message) { }
}