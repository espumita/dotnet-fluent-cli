namespace Dotnet.Cli.Args; 

public class CliArgsBuilder {
    private readonly string[] arg;

    private CliArgsBuilder(string[] arg) {
        this.arg = arg;
    }

    public static CliArgsBuilder From(string[] args) {
        return new CliArgsBuilder(args);
    }

    public ArgsOptions Build() {
        return new ArgsOptions();
    }
}

public class ArgsOptions {
    public ArgsOptions() {
        Flags = new List<FlagOption>();
    }

    public List<FlagOption> Flags { get; set; }
}

public class FlagOption {
}