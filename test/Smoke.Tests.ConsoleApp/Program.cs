using Dotnet.Cli.Args;

var environmentArgs = Environment.GetCommandLineArgs();

var argsOptions = CliArgsBuilder.From(environmentArgs)
    .AddFlag(config => config.ShortName = "help")
    .Build();

var forceFlagOption = argsOptions.Flag("help");

if (forceFlagOption.IsPresent) {
    Console.WriteLine("Help option enabled");
} else {
    Console.WriteLine("Running in normal mode");
}