namespace Fluent.Cli;

public class CliArguments {
    public List<Option> Options { get; }

    public CliArguments(List<Option> options) {
        Options = options;
    }

    public Option Option(char shortName) {
        var optionByShortName = Options.FirstOrDefault(option => shortName.Equals(option.ShortName));
        if (optionByShortName != null) return optionByShortName;
        throw new OptionIsNotConfiguredException($"Option -- '{shortName}' has not been configured yet, add it to the builder first.");
    }

    public Option Option(string longName) {
        var optionByName = Options.FirstOrDefault(optionByName => !string.IsNullOrEmpty(optionByName.Name) && optionByName.Name.Equals(longName));
        if (optionByName != null) return optionByName;
        throw new OptionIsNotConfiguredException($"Option -- '{longName}' has not been configured yet, add it to the builder first.");
    }

}