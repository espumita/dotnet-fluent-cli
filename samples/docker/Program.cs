using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var cliArguments = CliArgumentsBuilder.With(environmentArgs)
    .WithProgramHeaderDescription("A self-sufficient runtime for containers")
    .LongOption("config")
        .WithDescription("Location of client config files (default C:\\Users\\Home\\.docker\")")
        .WithArgument("string")
    .Option('c', "context")
        .WithDescription("Name of the context to use to connect to the daemon(overrides DOCKER_HOST env var and default context set with \"docker context use\")")
        .WithArgument("string")
    .Option('D', "debug")
        .WithDescription("Enable debug mode")
    .Option('H', "host")
        .WithDescription("Daemon socket(s) to connect to")
        .WithArgument("list")
    .Option('l', "log-level")
        .WithDescription("Set the logging level (\"debug\" | \"info\" | \"warn\" | \"error\" | \"fatal\") (default \"info\")")
        .WithArgument("string")
    .LongOption("tls")
        .WithDescription("Use TLS; implied by --tlsverify")
    .LongOption("tlscacert")
        .WithDescription("Trust certs signed only by this CA (default \"C:\\Users\\Home\\.docker\\ca.pem\")")
        .WithArgument("string")
    .LongOption("tlscert")
        .WithDescription("Path to TLS certificate file (default \"C:\\Users\\Home\\.docker\\cert.pem\")")
        .WithArgument("string")
    .LongOption("tlskey")
        .WithDescription("Path to TLS key file (default \"C:\\Users\\Home\\.docker\\key.pem\")")
        .WithArgument("string")
    .LongOption("tlsverify")
        .WithDescription("Use TLS and verify the remote")
    .Option('v', "version")
        .WithDescription("Print version information and quit")
    .Command("builder")
        .WithDescription("Manage builds")
    .Command("buildx")
        .WithDescription("Docker Buildx (Docker Inc., v0.7.1)")
    .Command("compose")
        .WithDescription("Docker Compose (Docker Inc., v2.2.3)")
    .Command("config")
        .WithDescription("Manage Docker configs")
    .Command("container")
        .WithDescription("Manage containers")
    .Command("context")
        .WithDescription("Manage contexts")
    .Command("image")
        .WithDescription(" Manage images")
    .Command("manifest")
        .WithDescription(" Manage Docker image manifests and manifest lists")
    .Command("network")
        .WithDescription("Manage networks")
    .Command("node")
        .WithDescription("Manage Swarm nodes")
    .Command("plugin")
        .WithDescription("Manage plugins")
    .Command("scan")
        .WithDescription("Docker Scan (Docker Inc., v0.17.0)")
    .Command("secret")
        .WithDescription("Manage Docker secrets")
    .Command("service")
        .WithDescription("Manage services")
    .Command("stack")
        .WithDescription("Manage Docker stacks")
    .Command("swarm")
        .WithDescription("Manage Swarm")
    .Command("system")
        .WithDescription("Manage Docker")
    .Command("trust")
        .WithDescription("Manage trust on Docker images")
    .Command("volume")
        .WithDescription("Manage volumes")
    //.Command("attach")
    //.Command("build")
    //.Command("commit")
    //.Command("cp")
    //.Command("create")
    //.Command("diff")
    //.Command("events")
    //.Command("exec")
    //.Command("export")
    //.Command("history")
    //.Command("images")
    //.Command("import")
    //.Command("info")
    //.Command("inspect")
    //.Command("kill")
    //.Command("load")
    //.Command("login")
    //.Command("logout")
    //.Command("logs")
    //.Command("pause")
    //.Command("port")
    //.Command("ps")
    //.Command("pull")
    //.Command("push")
    //.Command("rename")
    //.Command("restart")
    //.Command("rm")
    //.Command("rmi")
    //.Command("run")
    //.Command("save")
    //.Command("search")
    //.Command("start")
    //.Command("stats")
    //.Command("stop")
    //.Command("tag")
    //.Command("top")
    //.Command("unpause")
    //.Command("update")
    //.Command("version")
    //.Command("wait")
    .WithProgramFooterDescription(DockerProgramFooterDescription())
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

string DockerProgramFooterDescription() {
    return @"Run 'docker COMMAND --help' for more information on a command.

To get more help with docker, check out our guides at https://docs.docker.com/go/guides/
";
}