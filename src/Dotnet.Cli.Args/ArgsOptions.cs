namespace Dotnet.Cli.Args;

public class ArgsOptions {
    public List<FlagOption> Flags { get; set; }

    public ArgsOptions() {
        Flags = new List<FlagOption>();
    }

    public FlagOption Flag(string shortName) {
        return Flags.First(flag => flag.ShortName.Equals(shortName));
    }
}