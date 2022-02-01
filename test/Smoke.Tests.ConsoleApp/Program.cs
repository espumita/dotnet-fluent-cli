using Dotnet.Cli.Args;

var environmentArgs = Environment.GetCommandLineArgs();

var argsOptions = CliArgsBuilder.From(environmentArgs)
    .AddFlag(config => config.ShortName = "verbose")
    .Build();

if (argsOptions.Flag("verbose").IsPresent) {
    Console.Write("Verbose flag is present");
} else {
    Console.Write("Verbose flag is not present");
}

Environment.Exit(0);