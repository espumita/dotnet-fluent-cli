namespace Fluent.Cli.Exceptions;

public class OptionIsNotConfiguredException : CliArgumentsBuilderConfigurationException {
    public OptionIsNotConfiguredException(string message) : base(message) { }
}