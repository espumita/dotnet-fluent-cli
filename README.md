# Fluent.cli

Fluent interface to parse and configure arguments for command line applications in .NET.

## Types of standars suported

* [POSIX](https://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html), UNIX  or  short-option like options, for example `ls -la ~/.docker` 
* [GNU](https://www.gnu.org/software/libc/manual/html_node/Argument-Syntax.html) like long options, for example `ls --all --show-control-chars ~/.docker` are not supported yet.
* Traditional style like options, for example `ls la ~/.docker` are not supported yet.
* Java like properties `-com.java.property` are not supported yet.
* Windows style like options, for example `ls /all ~/.docker` are not supported yet.

---

## Ussage
`CliArgumentsBuilder` class let you dynamically, configure options. Once build, it will parse the `strin[] args` and returns a `CliArguments` object.

You can get program's arguments in .NET from the [Main entrypoint](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line#:~:text=The%20Main%20method%20is%20the,point%20in%20a%20C%23%20program.) `strin[] args` parameter, or just call `Environment.GetCommandLineArgs()`.

### Short options:
```c#
var cliArguments = CliArgumentsBuilder.With(args)
    .Option('a')
    .Build();

var option = cliArguments.Option('a');
if (option.IsPresent) {
    Console.WriteLine("Option 'a' enabled!");
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

* Prefixes can only be [Hyphen-minus](https://en.wikipedia.org/wiki/Hyphen-minus), U+002D character. One for sort options like `-c`, and two for long options like `--color`.

* \u2010-\u2015\u2212\uFE58\uFE63\uFF0D Are Invalid dashs for options.

* Only [unicode Basic Latin](https://en.wikipedia.org/wiki/Basic_Latin_(Unicode_block)) alphanumeric upper and lower case characters are acepted for short and long options. `-m`, `-2`, `-M`, `--mytest`, `--mytest01` or `--MyTest01` are valid options. Long options can also contain Hyphen-minus, U+002D character between words, for example: `--my-test-option`.

* All controlled exceptions ocurred during the argument parsing step are considered as [ArgumentException](https://docs.microsoft.com/es-mx/dotnet/api/system.argumentexception?view=net-6.0). While all controlled exceptions ocurred during the builder configuration step are considered as `CliArgumentsBuilderConfigurationException`.

## Under the dotnet Hood

* This implementation assumes that program console arguments comes directly splited and without spaces, character U+0020 removed. This 'split' is built in at the assambly code of the  c++ languje implementation, more info here: [Iso_C_1999, 5.1.2.2.1 Program startup](https://www.dii.uchile.cl/~daespino/files/Iso_C_1999_definition.pdf).

* Acording to Microsoft [corerun.hpp](https://github.com/dotnet/runtime/blob/994d390c7cdc1f91b2b37235cf68605ead5d7c44/src/coreclr/hosts/corerun/corerun.hpp) source code, [native c++ main function](https://en.cppreference.com/w/cpp/language/main_function) is replaced by `wmain` from `Windows.h` when runing in Windows and mantaining the native `main` in others cases like Linux.

*  The C# implementation of the diferent [Main Entrypoints](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line#:~:text=The%20Main%20method%20is%20the,point%20in%20a%20C%23%20program.), [GetCommandLineArgs](https://source.dot.net/System.Private.CoreLib/R/84c2c7cb5c89246f.html) and others. 
[System.Environment](https://source.dot.net/#System.Private.CoreLib/src/System/Environment.CoreCLR.cs,84c2c7cb5c89246f,references) points to this pointer under the hood. 
There are some Unicode checks at some point, but we can assume that all characters will be coded as two bytes in UTF-16 as C# native `System.S tring`.



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