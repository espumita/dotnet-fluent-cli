using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var cliArguments = CliArgumentsBuilder.With(environmentArgs)
    .LongOption("verbose")
    .LongOption("show-program-name")
    .LongOption("show-program-version")
    .LongOption("show-arguments")
    .Build();

if (cliArguments.IsOptionPresent("verbose")) {
    Console.Write("Verbose option is present");
}

if (cliArguments.IsOptionPresent("show-program-name")) {
    Console.Write(cliArguments.Program);
}

if (cliArguments.IsOptionPresent("show-program-version")) {
    Console.Write(cliArguments.Version);
}

if (cliArguments.IsOptionPresent("show-arguments")) {
    cliArguments.Arguments.ForEach(argument => {
        Console.Write($"{argument.Name}:{argument.Value}:");
    });
}

Environment.Exit(0);