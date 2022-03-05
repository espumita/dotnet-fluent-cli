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

PrintOption('a', "all");
PrintOption('A', "almost-all");
PrintLongOption("author");
PrintOption('b', "escape");
PrintLongOption("block-size");
PrintOption('B', "ignore-backups");
PrintOption('c');
PrintOption('C');
PrintLongOption("color");
PrintOption('d', "directory");
PrintOption('D', "dired");
PrintOption('f');
PrintOption('F', "classify");
PrintLongOption("file-type");
PrintLongOption("format");
PrintLongOption("full-time");
PrintOption('g');
PrintLongOption("group-directories-first");
PrintOption('G', "no-group");
PrintOption('h', "human-readable");
PrintLongOption("si");
PrintOption('H', "dereference-command-line");
PrintLongOption("dereference-command-line-symlink-to-dir");
PrintLongOption("hide");
PrintLongOption("hyperlink");
PrintLongOption("indicator-style");
PrintOption('i', "inode");
PrintOption('I', "ignore");
PrintOption('k', "kibibytes");
PrintOption('l');
PrintOption('L', "dereference");
PrintOption('m');
PrintOption('n', "numeric-uid-gid");
PrintOption('N', "literal");
PrintOption('o');
PrintOption('p', "indicator-style");
PrintOption('q', "hide-control-chars");
PrintLongOption("show-control-chars");
PrintOption('Q', "quote-name");
PrintLongOption("quoting-style");
PrintOption('r', "reverse");
PrintOption('R', "recursive");
PrintOption('s');
PrintLongOption("sort");
PrintLongOption("time");
PrintLongOption("time-style");
PrintOption('t');
PrintOption('T', "tabsize");
PrintOption('u');
PrintOption('U');
PrintOption('v');
PrintOption('w', "width");
PrintOption('x');
PrintOption('X');
PrintOption('Z', "context");
PrintOption('1');
PrintLongOption("help");
PrintLongOption("version");
PrintAllFilesArguments();

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
        var option = cliArguments.Option(optionLongName);
        if (cliArguments.IsOptionPresent(optionLongName)) {
            Console.WriteLine($"{optionLongName} option enabled");
            if (option.IsArgumentPresent()) {
                var argument = option.Argument();
                Console.WriteLine($"Argument {argument.Name} is present with value {argument.Value}");
            }
        }
    }
}

void PrintLongOption(string optionLongName) {
    PrintOption(optionLongName: optionLongName);
}

void PrintAllFilesArguments() {
    cliArguments.Arguments.ForEach(argument => {
        Console.WriteLine($"Argument:{argument.Name} - FILE: {argument.Value}");
    });
}