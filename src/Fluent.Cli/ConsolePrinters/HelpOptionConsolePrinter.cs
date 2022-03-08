using Fluent.Cli.Configuration;
using Fluent.Cli.Definitions;

namespace Fluent.Cli.ConsolePrinters;

public class HelpOptionConsolePrinter {

    public void PrintHelpAndStopProcess(string programName, IDictionary<string, OptionConfiguration> optionsConfigurations, CommandsDefinitions commandsDefinitions) {
        Console.Write(Environment.NewLine);
        var options = optionsConfigurations.Any() ? "[OPTIONS] " : string.Empty;
        var commands = commandsDefinitions.Definitions.Any() ? "[COMMAND] " : string.Empty;
        var arguments = true ? "[ARGUMENTS]" : string.Empty; // enabled by default
        var usageLie = $"Usage: {programName} {options}{commands}{arguments}";
        Console.WriteLine(usageLie);
        Console.Write(Environment.NewLine);
        Console.WriteLine("_____________________________________________________"); //program description
        if (optionsConfigurations.Any()) {
            Console.Write(Environment.NewLine);
            Console.WriteLine("Options:");
            foreach (var optionConfiguration in optionsConfigurations.Values) {
                var shortOptionName = ShortOptionName(optionConfiguration.PrimaryName);
                var comma = Comma(optionConfiguration.PrimaryName, optionConfiguration.SecondaryName);
                var longOptionName = LongOptionName(optionConfiguration.SecondaryName);
                var optionArgument = OptionArgument(optionConfiguration.Argument);
                var optionLine = $"  {shortOptionName}{comma} {longOptionName} {optionArgument}";
                if (optionLine.Length <= 27) {
                    var optionLineWithFirstColumnWithPadding = optionLine.PadRight(27, ' ');
                    var optionLineWithSecondColumnWithPadding = optionLineWithFirstColumnWithPadding.PadRight(80, '_'); //option description
                    Console.WriteLine(optionLineWithSecondColumnWithPadding);
                } else {
                    Console.WriteLine(optionLine);
                }

            }
        }
        if (commandsDefinitions.Definitions.Any()) {
            Console.Write(Environment.NewLine);
            Console.WriteLine("Commands:");
            foreach (var commandDefinition in commandsDefinitions.Definitions) {
                var command = commandDefinition.Key;
                var commandLine = $"  {command}";
                var commandLineWithFirstColumnWithPadding = commandLine.PadRight(14, ' ');
                var commandLineWithSecondColumnWithPadding = commandLineWithFirstColumnWithPadding.PadRight(80, '_'); //selectedCommand description
                Console.WriteLine(commandLineWithSecondColumnWithPadding);
            }
        }
        Console.Write(Environment.NewLine);
        Environment.Exit(0);
    }

    private string Comma(string shortOptionName, string longOptionName) {
        return !string.IsNullOrEmpty(shortOptionName) && !string.IsNullOrEmpty(longOptionName)
            ? ","
            : " ";
    }

    private string ShortOptionName(string shortOptionName) {
        return !string.IsNullOrEmpty(shortOptionName)
            ? $"-{shortOptionName}"
            : "  ";
    }

    private string LongOptionName(string longOptionName) {
        return !string.IsNullOrEmpty(longOptionName)
            ? $"--{longOptionName}"
            : string.Empty;
    }

    private string OptionArgument(ArgumentConfiguration optionArgument) {
        return !string.IsNullOrEmpty(optionArgument?.ArgumentName)
            ? optionArgument?.ArgumentName
            : string.Empty;
    }
}