namespace Fluent.Cli;

public class CliArguments {
    public List<Option> Options { get; }

    public CliArguments(List<Option> options) {
        Options = options;
    }

    public Option Option(string shortName) {
        var option = Options.FirstOrDefault(option => option.ShortName.Equals(shortName));
        if (option == null) throw new OptionIsNotConfiguredException($"Option -- '{shortName}' has not been configured yet, add it to the builder first.");
        return option;
    }


}