using Fluent.Cli.Exceptions;

namespace Fluent.Cli;

public class CliArguments {
    public List<Option> Options { get; }
    public List<Argument> Arguments { get; }

    public CliArguments(List<Option> options, List<Argument> arguments) {
        Options = options;
        Arguments = arguments;
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