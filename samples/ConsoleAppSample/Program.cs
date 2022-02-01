using Dotnet.Cli.Args;

var environmentArgs = Environment.GetCommandLineArgs();

var argsOptions = CliArgsBuilder.From(environmentArgs)
    .AddFlag(config => config.ShortName = "force")
    .Build();

if (argsOptions.Flag("force").IsPresent) {
    Console.WriteLine("Running in force mode");
} else {
    Console.WriteLine("Running in normal mode");
}
