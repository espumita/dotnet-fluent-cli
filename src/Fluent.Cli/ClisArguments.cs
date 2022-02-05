namespace Fluent.Cli;

public class ClisArguments {
    public List<Option> Options { get; set; }

    public ClisArguments() {
        Options = new List<Option>();
    }

    public Option Option(string shortName) {
        return Options.First(option => option.ShortName.Equals(shortName));
    }
}