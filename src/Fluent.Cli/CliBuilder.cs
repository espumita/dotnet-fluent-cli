namespace Fluent.Cli; 

public class CliBuilder {
    private readonly List<string> environmentArgs;
    private List<OptionConfiguration> OptionConfigurations;

    private CliBuilder(string[] environmentArgs) {
        this.environmentArgs = environmentArgs.ToList();
        OptionConfigurations = new List<OptionConfiguration>();
    }

    public static CliBuilder From(string[] args) {
        return new CliBuilder(args);
    }

    public CliBuilder Option(Action<OptionConfiguration> optionConfiguration) {
        var configuration = new OptionConfiguration();
        optionConfiguration(configuration);
        OptionConfigurations.Add(configuration);
        return this;
    }

    public CliBuilder Option(string shortName) {
        var configuration = new OptionConfiguration {
            ShortName = shortName
        };
        OptionConfigurations.Add(configuration);
        return this;
    }

    public CliBuilder Option(params string[] names) {
        var configuration = new OptionConfiguration {
            Names = names
        };
        OptionConfigurations.Add(configuration);
        return this;
    }

    public ClisArguments Build() {
        var clisArguments = new ClisArguments {
            Options = Options()
        };
        return clisArguments;
    }

    private List<Option> Options() {
        return OptionConfigurations
            .Select(configuration => new Option {
                ShortName = configuration.ShortName,
                IsPresent = IsPresent(configuration.ShortName)
            }).ToList();
    }

    private bool IsPresent(string shortName) {
        return environmentArgs.Any(arg => 
            arg.Equals(shortName) 
            || arg.Equals($"-{shortName}")
            || arg.Equals($"--{shortName}")
        );
    }
}