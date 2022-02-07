namespace Fluent.Cli;

public class ClisArguments {
    public List<Option> Options { get; }

    public ClisArguments(List<Option> options) {
        Options = options;
    }

    public Option Option(string shortName) {
        return Options.First(option => option.ShortName.Equals(shortName));
    }


}