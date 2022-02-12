using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var cli = CliArgumentsBuilder.With(environmentArgs)
    .Option('a', "all")
    .Option('A', "almost-all")
    .LongOption("author")
    .Option('b', "escape")
    .LongOption("block-size")
    .Option('B', "ignore-backups")
    .Option('c')
    .Option('C')
    .LongOption("color")
    .Option('d', "directory")
    .Option('D', "dired")
    .Option('f')
    .Option('F', "classify")
    .LongOption("file-type")
    .LongOption("format")
    .LongOption("full-time")
    .Option('g')
    .LongOption("group-directories-first")
    .Option('G', "no-group")
    .Option('h', "human-readable")
    .LongOption("si")
    .Option('H', "dereference-command-line")
    .LongOption("dereference-command-line-symlink-to-dir")
    .LongOption("hide")
    .LongOption("hyperlink")
    .LongOption("indicator-style")
    .Option('i', "inode")
    .Option('I', "ignore")
    .Option('k', "kibibytes")
    .Option('l')
    .Option('L', "dereference")
    .Option('m')
    .Option('n', "numeric-uid-gid")
    .Option('N', "literal")
    .Option('o')
    .Option('p', "indicator-style")
    .Option('q', "hide-control-chars")
    .LongOption("show-control-chars")
    .Option('Q', "quote-name")
    .LongOption("quoting-style")
    .Option('r', "reverse")
    .Option('R', "recursive")
    .Option('s')
    .LongOption("sort")
    .LongOption("time")
    .LongOption("time-style")
    .Option('t')
    .Option('T', "tabsize")
    .Option('u')
    .Option('U')
    .Option('v')
    .Option('w', "width")
    .Option('x')
    .Option('X')
    .Option('Z', "context")
    .Option('1')
    .LongOption("help")
    .LongOption("version")
    .Build();

ConfigureOption("a", "all");
ConfigureOption("A", "almost-all");
ConfigureOption("author");
ConfigureOption("b", "escape");
ConfigureOption("block-size");
ConfigureOption("B", "ignore-backups");
ConfigureOption("c");
ConfigureOption("C");
ConfigureOption("color");
ConfigureOption("d", "directory");
ConfigureOption("D", "dired");
ConfigureOption("f");
ConfigureOption("F", "classify");
ConfigureOption("file-type");
ConfigureOption("format");
ConfigureOption("full-time");
ConfigureOption("g");
ConfigureOption("group-directories-first");
ConfigureOption("G", "no-group");
ConfigureOption("h", "human-readable");
ConfigureOption("si");
ConfigureOption("H", "dereference-command-line");
ConfigureOption("dereference-command-line-symlink-to-dir");
ConfigureOption("hide");
ConfigureOption("hyperlink");
ConfigureOption("indicator-style");
ConfigureOption("i", "inode");
ConfigureOption("I", "ignore");
ConfigureOption("k", "kibibytes");
ConfigureOption("l");
ConfigureOption("L", "dereference");
ConfigureOption("m");
ConfigureOption("n", "numeric-uid-gid");
ConfigureOption("N", "literal");
ConfigureOption("o");
ConfigureOption("p", "indicator-style");
ConfigureOption("q", "hide-control-chars");
ConfigureOption("show-control-chars");
ConfigureOption("Q", "quote-name");
ConfigureOption("quoting-style");
ConfigureOption("r", "reverse");
ConfigureOption("R", "recursive");
ConfigureOption("s");
ConfigureOption("sort");
ConfigureOption("time");
ConfigureOption("time-style");
ConfigureOption("t");
ConfigureOption("T", "tabsize");
ConfigureOption("u");
ConfigureOption("U");
ConfigureOption("v");
ConfigureOption("w", "width");
ConfigureOption("x");
ConfigureOption("X");
ConfigureOption("Z", "context");
ConfigureOption("1");
ConfigureOption("help");
ConfigureOption("version");

Environment.Exit(0);

void ConfigureOption(params string[] optionShortNames) {
    foreach (var optionShortName in optionShortNames)
        if (cli.Option(optionShortName).IsPresent)
            Console.WriteLine($"{optionShortName} option enabled");
}