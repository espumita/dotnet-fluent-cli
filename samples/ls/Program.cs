using Fluent.Cli;

var environmentArgs = Environment.GetCommandLineArgs();

var cliArguments = CliArgumentsBuilder.With(environmentArgs)
    .WithProgramHeaderDescription(LsProgramHeaderDescription())
    .Option('a', "all")
        .WithDescription("do not ignore entries starting with .")
    .Option('A', "almost-all")
        .WithDescription("do not list implied . and ..")
    .LongOption("author")
        .WithDescription("with -l, print the author of each file")
    .Option('b', "escape")
        .WithDescription("print C-style escapes for nongraphic characters")
    .LongOption("block-size")
        .WithDescription("with -l, scale sizes by SIZE when printing them; e.g., '--block-size=M'; see SIZE format below")
        .WithArgument("SIZE")
    .Option('B', "ignore-backups")
        .WithDescription("do not list implied entries ending with ~")
    .Option('c')
        .WithDescription("with -lt: sort by, and show, ctime (time of last modification of file status information); with - l: show ctime and sort by name; otherwise: sort by ctime, newest first")
    .Option('C')
        .WithDescription("list entries by columns")
    .LongOption("color")
        .WithDescription("colorize the output; WHEN can be 'always' (default if omitted), 'auto', or 'never'; more info below")
        .WithArgument("WHEN")
    .Option('d', "directory")
        .WithDescription("list directories themselves, not their contents")
    .Option('D', "dired")
        .WithDescription("generate output designed for Emacs' dired mode")
    .Option('f')
        .WithDescription("do not sort, enable -aU, disable -ls --color")
    .Option('F', "classify")
        .WithDescription("append indicator (one of */=>@|) to entries")
    .LongOption("file-type")
        .WithDescription("likewise, except do not append '*'")
    .LongOption("format")
        .WithDescription("across -x, commas -m, horizontal -x, long -l, single - column - 1, verbose - l, vertical - C")
        .WithArgument("WORD")
    .LongOption("full-time")
        .WithDescription("like -l --time-style=full-iso")
    .Option('g')
        .WithDescription("like -l, but do not list owner")
    .LongOption("group-directories-first")
        .WithDescription("group directories before files; can be augmented with a--sort option, but any use of --sort = none(-U) disables grouping")
    .Option('G', "no-group")
        .WithDescription("in a long listing, don't print group names")
    .Option('h', "human-readable")
        .WithDescription("with -l and -s, print sizes like 1K 234M 2G etc.")
    .LongOption("si")
        .WithDescription("likewise, but use powers of 1000 not 1024")
    .Option('H', "dereference-command-line")
        .WithDescription("follow symbolic links listed on the command line")
    .LongOption("dereference-command-line-symlink-to-dir")
        .WithDescription("follow each command line symbolic link that points to a directory")
    .LongOption("hide")
        .WithDescription("do not list implied entries matching shell PATTERN (overridden by - a or - A)")
        .WithArgument("PATTERN")
    .LongOption("hyperlink")
        .WithDescription("hyperlink file names; WHEN can be 'always' (default ifomitted), 'auto', or 'never'")
        .WithArgument("WHEN")
    .LongOption("indicator-style")
        .WithDescription("append indicator with style WORD to entry names: none (default), slash(-p), file - type(--file - type), classify (-F)")
        .WithArgument("WORD")
    .Option('i', "inode")
        .WithDescription("print the index number of each file")
    .Option('I', "ignore")
        .WithDescription("do not list implied entries matching shell PATTERN")
        .WithArgument("PATTERN")
    .Option('k', "kibibytes")
        .WithDescription("default to 1024-byte blocks for disk usage; used only with - s and per directory totals")
    .Option('l')
        .WithDescription("use a long listing format")
    .Option('L', "dereference")
        .WithDescription("when showing file information for a symbolic link, show information for the file the link references rather than for the link itself")
    .Option('m')
        .WithDescription("fill width with a comma separated list of entries")
    .Option('n', "numeric-uid-gid")
        .WithDescription(" like -l, but list numeric user and group IDs")
    .Option('N', "literal")
        .WithDescription("print entry names without quoting")
    .Option('o')
        .WithDescription("like -l, but do not list group information")
    .Option('p')
        .WithDescription("append / indicator to directories")
    .Option('q', "hide-control-chars")
        .WithDescription("print ? instead of nongraphic characters")
    .LongOption("show-control-chars")
        .WithDescription("show nongraphic characters as-is (the default, unless program is 'ls' and output is a terminal)")
    .Option('Q', "quote-name")
        .WithDescription("enclose entry names in double quotes")
    .LongOption("quoting-style")
        .WithDescription("use quoting style WORD for entry names: literal, locale, shell, shell - always, shell - escape, shell - escape - always, c, escape(overrides QUOTING_STYLE environment variable)")
        .WithArgument("WORD")
    .Option('r', "reverse")
        .WithDescription("reverse order while sorting")
    .Option('R', "recursive")
        .WithDescription("list subdirectories recursively")
    .Option('s', "size")
        .WithDescription("print the allocated size of each file, in blocks")
    .Option('S')
        .WithDescription("sort by file size, largest first")
    .LongOption("sort")
        .WithDescription("sort by WORD instead of name: none (-U), size (-S), time (-t), version(-v), extension(-X)")
        .WithArgument("WORD")
    .LongOption("time")
        .WithDescription("change the default of using modification times; access time(-u): atime, access, use; change time(-c): ctime, status; birth time: birth, creation; with - l, WORD determines which time to show; with --sort = time, sort by WORD (newest first)")
        .WithArgument("WORD")
    .LongOption("time-style")
        .WithDescription("time/date format with -l; see TIME_STYLE below")
        .WithArgument("TIME_STYLE")
    .Option('t')
        .WithDescription("sort by time, newest first; see --time")
    .Option('T', "tabsize")
        .WithDescription("assume tab stops at each COLS instead of 8")
        .WithArgument("COLS")
    .Option('u')
        .WithDescription("with -lt: sort by, and show, access time; with -l: show access time and sort by name; otherwise: sort by access time, newest first")
    .Option('U')
        .WithDescription("do not sort; list entries in directory order")
    .Option('v')
        .WithDescription("natural sort of (version) numbers within text")
    .Option('w', "width")
        .WithDescription("set output width to COLS.  0 means no limit")
        .WithArgument("COLS")
    .Option('x')
        .WithDescription("list entries by lines instead of by columns")
    .Option('X')
        .WithDescription("sort alphabetically by entry extension")
    .Option('Z', "context")
        .WithDescription("print any security context of each file")
    .Option('1')
        .WithDescription("list one file per line.  Avoid '\\n' with -q or -b")
    .LongOption("help")
        .WithDescription("display this help and exit")
    .LongOption("version")
        .WithDescription("output version information and exit")
    .WithProgramFooterDescription(LsProgramFooterDescription())
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

string LsProgramHeaderDescription() {
    return @"List information about the FILEs (the current directory by default).
Sort entries alphabetically if none of -cftuvSUX nor --sort is specified.

Mandatory arguments to long options are mandatory for short options too.";
}

string LsProgramFooterDescription() {
    return @"The SIZE argument is an integer and optional unit (example: 10K is 10*1024).
Units are K,M,G,T,P,E,Z,Y (powers of 1024) or KB,MB,... (powers of 1000).

The TIME_STYLE argument can be full-iso, long-iso, iso, locale,or +FORMAT.
FORMAT is interpreted like in date(1).  If FORMAT is FORMAT1<newline>FORMAT2,
then FORMAT1 applies to non-recent files and FORMAT2 to recent files.
TIME_STYLE prefixed with 'posix-' takes effect only outside the POSIX locale.
Also the TIME_STYLE environment variable sets the default style to use.

Using color to distinguish file types is disabled both by default and
with --color=never.  With --color=auto, ls emits color codes only when
standard output is connected to a terminal. The LS_COLORS environment
variable can change the settings. Use the dircolors command to set it.

Exit status:
 0  if OK,
 1  if minor problems (e.g., cannot access subdirectory),
 2  if serious trouble (e.g., cannot access command-line argument).

GNU coreutils online help: <https://www.gnu.org/software/coreutils/>
Report ls translation bugs to <https://translationproject.org/team/>
Full documentation at: <https://www.gnu.org/software/coreutils/ls>
or available locally via: info '(coreutils) ls invocation'
";
}