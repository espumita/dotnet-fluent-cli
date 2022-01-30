// See https://aka.ms/new-console-template for more information

using Dotnet.Cli.Args;

Console.WriteLine("Hello, World!");

var environmentArgs = Environment.GetCommandLineArgs();

var argsOptions = CliArgsBuilder.From(environmentArgs)
    .AddFlag(config => config.ShortName = "force")
    .Build();

var forceFlagOption = argsOptions.Flags.First(flag => flag.ShortName.Equals("force"));

if (forceFlagOption.IsPresent) {
    Console.WriteLine("Force mode is enabled");
} else {
    Console.WriteLine("Running in normal mode");
}
