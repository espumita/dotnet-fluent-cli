using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var cliArguments = CliArgumentsBuilder.With(environmentArgs)
    .LongOption("config")
        .WithArgument("string")
    .Option('c', "context")
        .WithArgument("string")
    .Option('D', "debug")
    .Option('H', "host")
        .WithArgument("list")
    .Option('l', "log-level")
        .WithArgument("string")
    .LongOption("tls")
    .LongOption("tlscacert")
        .WithArgument("string")
    .LongOption("tlscert")
        .WithArgument("string")
    .LongOption("tlskey")
        .WithArgument("string")
    .LongOption("tlsverify")
    .Option('v', "version")
    .Command("builder")
    .Command("buildx")
    .Command("compose")
    .Command("config")
    .Command("container")
    .Command("context")
    .Command("image")
    .Command("manifest")
    .Command("network")
    .Command("node")
    .Command("plugin")
    .Command("scan")
    .Command("secret")
    .Command("service")
    .Command("stack")
    .Command("swarm")
    .Command("system")
    .Command("trust")
    .Command("volume")
    .Command("attach")
    .Command("build")
    .Command("commit")
    .Command("cp")
    .Command("create")
    .Command("diff")
    .Command("events")
    .Command("exec")
    .Command("export")
    .Command("history")
    .Command("images")
    .Command("import")
    .Command("info")
    .Command("inspect")
    .Command("kill")
    .Command("load")
    .Command("login")
    .Command("logout")
    .Command("logs")
    .Command("pause")
    .Command("port")
    .Command("ps")
    .Command("pull")
    .Command("push")
    .Command("rename")
    .Command("restart")
    .Command("rm")
    .Command("rmi")
    .Command("run")
    .Command("save")
    .Command("search")
    .Command("start")
    .Command("stats")
    .Command("stop")
    .Command("tag")
    .Command("top")
    .Command("unpause")
    .Command("update")
    .Command("version")
    .Command("wait")
    .Build();

PrintLongOption("config");
PrintOption('c', "context");
PrintOption('D', "debug");
PrintOption('H', "host");
PrintOption('l', "log-level");
PrintLongOption("tls");
PrintLongOption("tlscacert");
PrintLongOption("tlscert");
PrintLongOption("tlskey");
PrintLongOption("tlsverify");
PrintOption('v',"version");
PrintCommands();
PrintAllArguments();

Environment.Exit(0);

void PrintOption(char? optionsShortName = null, string optionLongName = null) {
    if (optionsShortName != null) { 
        if (cliArguments.IsOptionPresent((char)optionsShortName)) {
            var option = cliArguments.Option((char)optionsShortName);
            Console.WriteLine($"{optionsShortName} option enabled");
            if (option.IsArgumentPresent()) {
                var argument = option.Argument();
                Console.WriteLine($"Argument {argument.Name} is present with value {argument.Value}");
            }
        }
    } 
    if (optionLongName != null) {
        if (cliArguments.IsOptionPresent(optionLongName)) {
            var longOption = cliArguments.Option(optionLongName);
            Console.WriteLine($"{longOption.Name} option enabled");
            if (longOption.IsArgumentPresent()) {
                var argument = longOption.Argument();
                Console.WriteLine($"Argument {argument.Name} is present with value {argument.Value}");
            }
        }
    }
}

void PrintLongOption(string optionLongName) {
    PrintOption(optionLongName: optionLongName);
}

void PrintCommands() {
    if (cliArguments.IsCommandPresent()) {
        var command = cliArguments.Command();
        Console.WriteLine($"{command.Name} command selected");
    }
}

void PrintAllArguments() {
    cliArguments.Arguments.ForEach(argument => {
        Console.WriteLine($"Argument:{argument.Name} - Value: {argument.Value}");
    });
}