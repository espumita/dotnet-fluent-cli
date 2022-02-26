namespace Fluent.Cli.Exceptions;

public class ArgumentIsNotConfiguredException : Exception {
    public ArgumentIsNotConfiguredException(string message) : base(message) { }
}