# Fluent.cli

Fluent interface to parse and configure arguments for command line applications in .NET.

## Types of standars suported

* [POSIX](https://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html), UNIX  or  short-option like options, for example `ls -la ~/.docker` 
* [GNU](https://www.gnu.org/software/libc/manual/html_node/Argument-Syntax.html) like long options, for example `ls --all --show-control-chars ~/.docker` 
* Traditional style like options, for example `ls la ~/.docker`
* Java like properties `-com.java.property` are not supported.
* Windows style like options, for example `ls /all ~/.docker` are not supported.

---

## Ussage and concept
`CliArgumentsBuilder` class let you dynamically, configure options. Once build, it will parse the `strin[] args` and return a `CliArguments` object.

You can get program's arguments in .NET from the [Main entrypoint](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line#:~:text=The%20Main%20method%20is%20the,point%20in%20a%20C%23%20program.) `strin[] args` parameter, or just call `Environment.GetCommandLineArgs()`.

### Short options:
```c#
public static void Main(string[] args) {

    var cliArguments = CliArgumentsBuilder.With(args)
        .Option('a')
        .Build();

    var option = cliArguments.Option('a');
    if (option.IsPresent) {
        Console.WriteLine("Option 'a' enabled!");
    }
}
```
### Long options:
```c#
var cliArguments = CliArgumentsBuilder.With(args)
    .LongOption("all")
    .Build();

var option = cliArguments.Option("all");
if (option.IsPresent) {
    Console.WriteLine("Option 'all' enabled!");
}
```
### Short and long options together:
```c#
var cliArguments = CliArgumentsBuilder.With(args)
    .Option('a', "all")
    .Build();

var shortOption = cliArguments.Option('a');
if (shortOption.IsPresent) {
    Console.WriteLine("Option 'a' enabled!");
}
var longOption = cliArguments.Option("all");
if (longOption.IsPresent) {
    Console.WriteLine("Option 'all' enabled!");
}
```
### Short/Long options with argument:
```c#
var cliArguments = CliArgumentsBuilder.With(args)
    .LongOption("block-size")
        .WithArgument("SIZE")
    .Build();

var option = cliArguments.Option("block-size");
if (option.IsPresent) {
    var argument = option.Argument();
    Console.WriteLine($"Option 'block-size' enabled with SIZE value:{argument.Value}");
}
```

---

**Considerations**

* Prefixes can only be [Hyphen-minus](https://en.wikipedia.org/wiki/Hyphen-minus), U+002D character. One for sort options `-r` and two for long options `--color`.

* Only [unicode Basic Latin](https://en.wikipedia.org/wiki/Basic_Latin_(Unicode_block)) alphanumeric upper and lower case characters are acepted for short and long options. Also, long options can contain Hyphen-minus, U+002D character between words, for example: `--my-test-option`.

* ?

---

## Installation

Install [Fluent.Cli](https://www.nuget.org/packages/Fluent.Cli/) nuget package or:

Using dotnet cli:

```
dotnet add package Fluent.Cli
```

or using nuget Package Manager:

```
Install-Package Fluent.Cli
```