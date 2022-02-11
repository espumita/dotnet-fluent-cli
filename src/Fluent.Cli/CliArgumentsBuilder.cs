namespace Fluent.Cli; 

public class CliArgumentsBuilder {
    private readonly string[] environmentArgs;
    private IDictionary<string, OptionConfiguration> optionConfigurations;

    private CliArgumentsBuilder(string[] environmentArgs) {
        this.environmentArgs = environmentArgs;
        optionConfigurations = new Dictionary<string, OptionConfiguration>();
    }

    public static CliArgumentsBuilder With(string[] args) {
        if (args == null) throw new ArgumentException("args cannot be null");
        return new CliArgumentsBuilder(args);
    }

    public CliArgumentsBuilder Option(string shortName) {
        var optionConfiguration = OptionConfiguration.For(shortName);
        optionConfigurations[shortName] = optionConfiguration;
        return this;
    }

    public CliArgumentsBuilder Option(string shortName, string name) {
        var optionConfiguration = OptionConfiguration.For(shortName, name);
        optionConfigurations[shortName] = optionConfiguration;
        return this;
    }

    public CliArguments Build() {
        return CliArgumentsParser.ParseFrom(environmentArgs, optionConfigurations);
    }


}