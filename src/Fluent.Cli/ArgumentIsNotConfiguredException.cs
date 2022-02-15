namespace Fluent.Cli;

public class ArgumentIsNotConfiguredException : Exception {
    public ArgumentIsNotConfiguredException(string message) : base(message) { }
}