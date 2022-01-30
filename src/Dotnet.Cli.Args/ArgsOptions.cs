namespace Dotnet.Cli.Args;

public class ArgsOptions {
    public ArgsOptions() {
        Flags = new List<FlagOption>();
    }

    public List<FlagOption> Flags { get; set; }
}