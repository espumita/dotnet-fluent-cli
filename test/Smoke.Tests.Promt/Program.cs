using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var cliArguments = CliArgumentsBuilder.With(environmentArgs)
    .LongOption("verbose")
    .LongOption("show-program-name")
    .LongOption("show-program-version")
    .LongOption("show-arguments")
    .Build();

if (cliArguments.Option("verbose").IsPresent) {
    Console.Write("Verbose option is present");
}

if (cliArguments.Option("show-program-name").IsPresent) {
    Console.Write(cliArguments.Program);
}

if (cliArguments.Option("show-program-version").IsPresent) {
    Console.Write(cliArguments.Version);
}

if (cliArguments.Option("show-arguments").IsPresent) {
    cliArguments.Arguments.ForEach(argument => {
        Console.Write($"{argument.Name}:{argument.Value}:");
    });
}

Environment.Exit(0);