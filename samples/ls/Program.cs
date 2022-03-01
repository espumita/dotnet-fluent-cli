using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var cliArguments = CliArgumentsBuilder.With(environmentArgs)
    .Option('a', "all")
    .Option('A', "almost-all")
    .LongOption("author")
    .Option('b', "escape")
    .LongOption("block-size")
        .WithOptionArgument("SIZE")
    .Option('B', "ignore-backups")
    .Option('c')
    .Option('C')
    .LongOption("color")
        .WithOptionArgument("WHEN")
    .Option('d', "directory")
    .Option('D', "dired")
    .Option('f')
    .Option('F', "classify")
    .LongOption("file-type")
    .LongOption("format")
        .WithOptionArgument("WORD")
    .LongOption("full-time")
    .Option('g')
    .LongOption("group-directories-first")
    .Option('G', "no-group")
    .Option('h', "human-readable")
    .LongOption("si")
    .Option('H', "dereference-command-line")
    .LongOption("dereference-command-line-symlink-to-dir")
    .LongOption("hide")
        .WithOptionArgument("PATTERN")
    .LongOption("hyperlink")
        .WithOptionArgument("WHEN")
    .LongOption("indicator-style")
        .WithOptionArgument("WORD")
    .Option('i', "inode")
    .Option('I', "ignore")
        .WithOptionArgument("PATTERN")
    .Option('k', "kibibytes")
    .Option('l')
    .Option('L', "dereference")
    .Option('m')
    .Option('n', "numeric-uid-gid")
    .Option('N', "literal")
    .Option('o')
    .Option('p')
    .Option('q', "hide-control-chars")
    .LongOption("show-control-chars")
    .Option('Q', "quote-name")
    .LongOption("quoting-style")
        .WithOptionArgument("WORD")
    .Option('r', "reverse")
    .Option('R', "recursive")
    .Option('s')
    .LongOption("sort")
        .WithOptionArgument("WORD")
    .LongOption("time")
        .WithOptionArgument("WORD")
    .LongOption("time-style")
        .WithOptionArgument("TIME_STYLE")
    .Option('t')
    .Option('T', "tabsize")
        .WithOptionArgument("COLS")
    .Option('u')
    .Option('U')
    .Option('v')
    .Option('w', "width")
        .WithOptionArgument("COLS")
    .Option('x')
    .Option('X')
    .Option('Z', "context")
    .Option('1')
    .LongOption("help")
    .LongOption("version")
    .Build();

ConfigureOption('a', "all");
ConfigureOption('A', "almost-all");
ConfigureLongOption("author");
ConfigureOption('b', "escape");
ConfigureLongOption("block-size");
ConfigureOption('B', "ignore-backups");
ConfigureOption('c');
ConfigureOption('C');
ConfigureLongOption("color");
ConfigureOption('d', "directory");
ConfigureOption('D', "dired");
ConfigureOption('f');
ConfigureOption('F', "classify");
ConfigureLongOption("file-type");
ConfigureLongOption("format");
ConfigureLongOption("full-time");
ConfigureOption('g');
ConfigureLongOption("group-directories-first");
ConfigureOption('G', "no-group");
ConfigureOption('h', "human-readable");
ConfigureLongOption("si");
ConfigureOption('H', "dereference-command-line");
ConfigureLongOption("dereference-command-line-symlink-to-dir");
ConfigureLongOption("hide");
ConfigureLongOption("hyperlink");
ConfigureLongOption("indicator-style");
ConfigureOption('i', "inode");
ConfigureOption('I', "ignore");
ConfigureOption('k', "kibibytes");
ConfigureOption('l');
ConfigureOption('L', "dereference");
ConfigureOption('m');
ConfigureOption('n', "numeric-uid-gid");
ConfigureOption('N', "literal");
ConfigureOption('o');
ConfigureOption('p', "indicator-style");
ConfigureOption('q', "hide-control-chars");
ConfigureLongOption("show-control-chars");
ConfigureOption('Q', "quote-name");
ConfigureLongOption("quoting-style");
ConfigureOption('r', "reverse");
ConfigureOption('R', "recursive");
ConfigureOption('s');
ConfigureLongOption("sort");
ConfigureLongOption("time");
ConfigureLongOption("time-style");
ConfigureOption('t');
ConfigureOption('T', "tabsize");
ConfigureOption('u');
ConfigureOption('U');
ConfigureOption('v');
ConfigureOption('w', "width");
ConfigureOption('x');
ConfigureOption('X');
ConfigureOption('Z', "context");
ConfigureOption('1');
ConfigureLongOption("help");
ConfigureLongOption("version");
ConfigureFilesArguments();

Environment.Exit(0);

void ConfigureOption(char? optionsShortName = null, string optionLongName = null) {
    if (optionsShortName != null) {
        var shortOption = cliArguments.Option((char) optionsShortName);
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

void ConfigureFilesArguments() {
    cliArguments.Arguments.ForEach(argument => {
        Console.WriteLine($"Argument:{argument.Name} - FILE: {argument.Value}");
    });
}