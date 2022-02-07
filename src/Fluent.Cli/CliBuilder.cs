namespace Fluent.Cli; 

public class CliBuilder {
    private readonly string[] environmentArgs;
    private IDictionary<string, OptionConfiguration> optionConfigurations;

    private CliBuilder(string[] environmentArgs) {
        this.environmentArgs = (string[]) environmentArgs.Clone();
        optionConfigurations = new Dictionary<string, OptionConfiguration>();
    }

    public static CliBuilder With(string[] args) {
        return new CliBuilder(args);
    }

    //public CliBuilder Option(Action<OptionConfiguration> optionConfiguration) {
    //    var configuration = new OptionConfiguration();
    //    optionConfiguration(configuration);
    //    optionConfigurations.Add(configuration);
    //    return this;
    //}

    public CliBuilder Option(string shortName) {
        var optionConfiguration = new OptionConfiguration(shortName);
        optionConfigurations[shortName] = optionConfiguration;
        return this;
    }

    //public CliBuilder Option(params string[] names) {
    //    var configuration = new OptionConfiguration {
    //        Names = names
    //    };
    //    optionConfigurations.Add(configuration);
    //    return this;
    //}

    public ClisArguments Build() {
        return ClisArgumentsParser.ParseFrom(environmentArgs, optionConfigurations);
    }


}