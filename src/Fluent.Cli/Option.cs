using Fluent.Cli.Exceptions;

namespace Fluent.Cli;

public class Option {
    public char? ShortName { get; }
    public string Name { get; }
    public bool IsPresent { get; }
    public Argument ArgumentPresent { get; }

    public Option(char? shortName, string name, bool isPresent) {
        ShortName = shortName;
        Name = name;
        IsPresent = isPresent;
    }

    public Option(char? shortName, string name, bool isPresent, string argumentName) {
        ShortName = shortName;
        Name = name;
        IsPresent = isPresent;
        ArgumentPresent = string.IsNullOrEmpty(argumentName) ? null : new Argument(argumentName, null);
    }

    public Option(char? shortName, string name, bool isPresent, string argumentName, string argumentValue) {
        ShortName = shortName;
        Name = name;
        IsPresent = isPresent;
        ArgumentPresent = new Argument(argumentName, argumentValue);
    }

    public Argument Argument() {
        if (ArgumentPresent != null) return ArgumentPresent;
        throw new ArgumentIsNotConfiguredException($"Argument for option '{(!string.IsNullOrEmpty(Name) ? Name : ShortName.ToString())}' has not been configured yet, add it to the builder first.");
    }

    public bool IsArgumentPresent() {
        return ArgumentPresent?.Name != null;
    }
}