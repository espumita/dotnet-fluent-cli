using Fluent.Cli.Exceptions;

namespace Fluent.Cli;

public class CliArguments {
    public string Program { get; }
    public Command? Command { get; }
    public List<Option> Options { get; }
    public List<Argument> Arguments { get; }

    public CliArguments(string program, Command? command, List<Option> options, List<Argument> arguments) {
        Program = program;
        Command = command;
        Options = options;
        Arguments = arguments;
    }

    public bool IsCommandPresent() {
        return Command != null;
    }

    public Command GetCommand() {
        return Command;
    }

    public Option Option(char shortName) {
        var option = Options.FirstOrDefault(option => shortName.Equals(option.ShortName));
        if (option != null) return option;
        throw new OptionIsNotConfiguredException($"Option -- '{shortName}' has not been configured yet, add it to the builder first.");
    }

    public Option Option(string longName) {
        var option = Options.FirstOrDefault(option => !string.IsNullOrEmpty(option.Name) && option.Name.Equals(longName));
        if (option != null) return option;
        throw new OptionIsNotConfiguredException($"Option -- '{longName}' has not been configured yet, add it to the builder first.");
    }

    public Argument Argument(string argumentName) {
        var argument = Arguments.FirstOrDefault(argument => argument.Name.Equals(argumentName));
        //TODO
        return argument;
    }
}