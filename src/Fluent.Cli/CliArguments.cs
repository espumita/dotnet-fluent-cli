namespace Fluent.Cli;

public class CliArguments {
    public List<Option> Options { get; }

    public CliArguments(List<Option> options) {
        Options = options;
    }

    public Option Option(string shortName) {
        var optionByShortName = Options.FirstOrDefault(option => option.ShortName.Equals(shortName));
        if (optionByShortName != null) return optionByShortName;
        var optionByName = Options.FirstOrDefault(optionByName => optionByName.Name.Equals(shortName));
        if (optionByName != null) return optionByName;
        throw new OptionIsNotConfiguredException($"Option -- '{shortName}' has not been configured yet, add it to the builder first.");
    }


}