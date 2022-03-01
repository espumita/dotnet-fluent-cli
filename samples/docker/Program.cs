using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var cliArguments = CliArgumentsBuilder.With(environmentArgs)
    .LongOption("config")
        .WithOptionArgument("string")
    .Option('c', "context")
        .WithOptionArgument("string")
    .Option('D', "debug")
    .Option('H', "host")
        .WithOptionArgument("list")
    .Option('l', "log-level")
        .WithOptionArgument("string")
    .LongOption("tls")
    .LongOption("tlscacert")
        .WithOptionArgument("string")
    .LongOption("tlscert")
        .WithOptionArgument("string")
    .LongOption("tlskey")
        .WithOptionArgument("string")
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

ConfigureLongOption("config");
ConfigureOption('c', "context");
ConfigureOption('D', "debug");
ConfigureOption('H', "host");
ConfigureOption('l', "log-level");
ConfigureLongOption("tls");
ConfigureLongOption("tlscacert");
ConfigureLongOption("tlscert");
ConfigureLongOption("tlskey");
ConfigureLongOption("tlsverify");
ConfigureOption('v',"version");
ConfigureCommands();
ConfigureFilesArguments();

Environment.Exit(0);

void ConfigureOption(char? optionsShortName = null, string optionLongName = null) {
    if (optionsShortName != null) {
        var shortOption = cliArguments.Option((char)optionsShortName);
        if (shortOption.IsPresent) {
            Console.WriteLine($"{optionsShortName} option enabled");
            if (shortOption._Argument?.Name != null)
                Console.WriteLine($"Argument {shortOption._Argument.Name} is present with value {shortOption._Argument.Value}");
        }
    }
    if (optionLongName != null) {
        var longOption = cliArguments.Option(optionLongName);
        if (longOption.IsPresent) {
            Console.WriteLine($"{optionLongName} option enabled");
            if (longOption._Argument?.Name != null)
                Console.WriteLine($"Argument {longOption._Argument.Name} is present with value {longOption._Argument.Value}");
        }
    }
}

void ConfigureLongOption(string optionLongName) {
    ConfigureOption(optionLongName: optionLongName);
}

void ConfigureCommands() {
    if (cliArguments.IsCommandPresent()) {
        var command = cliArguments.GetCommand();
        Console.WriteLine($"{command.Name} command selected");
    }
}

void ConfigureFilesArguments() {
    cliArguments.Arguments.ForEach(argument => {
        Console.WriteLine($"Argument:{argument.Name} - FILE: {argument.Value}");
    });
}