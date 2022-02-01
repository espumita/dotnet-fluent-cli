using Dotnet.Cli.Args;

var environmentArgs = Environment.GetCommandLineArgs();

var argsOptions = CliArgsBuilder.From(environmentArgs)
    .AddFlag(config => config.ShortName = "option")
    .Build();

if (argsOptions.Flag("option").IsPresent) {
    Console.Write("Option flag is present");
} else {
    Console.Write("Option flag is not present");
}

Environment.Exit(0);