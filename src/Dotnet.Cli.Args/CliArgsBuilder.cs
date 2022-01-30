namespace Dotnet.Cli.Args; 

public class CliArgsBuilder {
    private readonly List<string> args;
    private List<FlagOptionConfiguration> flagOptionConfigurations;

    private CliArgsBuilder(string[] args) {
        this.args = args.ToList();
        flagOptionConfigurations = new List<FlagOptionConfiguration>();
    }

    public static CliArgsBuilder From(string[] args) {
        return new CliArgsBuilder(args);
    }

    public CliArgsBuilder AddFlag(Action<FlagOptionConfiguration> setupConfiguration) {
        var optionConfiguration = new FlagOptionConfiguration();
        setupConfiguration(optionConfiguration);
        flagOptionConfigurations.Add(optionConfiguration);
        return this;
    }

    public ArgsOptions Build() {
        var argsOptions = new ArgsOptions {
            Flags = FlagsOptions()
        };
        return argsOptions;
    }

    private List<FlagOption> FlagsOptions() {
        return flagOptionConfigurations
            .Select(configuration => new FlagOption {
                ShortName = configuration.ShortName,
                IsPresent = IsPresent(configuration.ShortName)
            }).ToList();
    }

    private bool IsPresent(string flagShortName) {
        return args.Any(arg => 
            arg.Equals(flagShortName) 
            || arg.Equals($"-{flagShortName}")
            || arg.Equals($"--{flagShortName}")
        );
    }
}