namespace Fluent.Cli; 

public class CliBuilder {
    private readonly string[] environmentArgs;
    private IDictionary<string, OptionConfiguration> optionConfigurations;

    private CliBuilder(string[] environmentArgs) {
        this.environmentArgs = (string[]) environmentArgs.Clone();
        optionConfigurations = new Dictionary<string, OptionConfiguration>();
    }

    public static CliBuilder With(string[] args) {
        if (args == null) throw new ArgumentException("args cannot be null");
        return new CliBuilder(args);
    }

    public CliBuilder Option(string shortName) {
        var optionConfiguration = OptionConfiguration.For(shortName);
        optionConfigurations[shortName] = optionConfiguration;
        return this;
    }

    public ClisArguments Build() {
        return ClisArgumentsParser.ParseFrom(environmentArgs, optionConfigurations);
    }


}