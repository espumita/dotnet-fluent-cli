using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var argsOptions = CliBuilder.From(environmentArgs)
    .Option(config => config.ShortName = "verbose")
    .Build();

if (argsOptions.Option("verbose").IsPresent) {
    Console.Write("Verbose option is present");
} else {
    Console.Write("Verbose option is not present");
}

Environment.Exit(0);