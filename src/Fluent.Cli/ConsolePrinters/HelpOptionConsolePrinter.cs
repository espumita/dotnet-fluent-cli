using Fluent.Cli.Configuration;

namespace Fluent.Cli.ConsolePrinters;

public class HelpOptionConsolePrinter {

    public void PrintHelpAndStopProcess(string programName, IDictionary<string, OptionConfiguration> optionsConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations) {
        Console.Write(Environment.NewLine);
        PrintUsageLine(programName, optionsConfigurations, commandConfigurations);
        Console.Write(Environment.NewLine);
        PrintProgramDescription();
        if (optionsConfigurations.Any()) {
            Console.Write(Environment.NewLine);
            PrintOptions(optionsConfigurations);
        }
        if (commandConfigurations.Any()) {
            Console.Write(Environment.NewLine);
            PrintCommands(commandConfigurations);
        }
        Console.Write(Environment.NewLine);
        Environment.Exit(0);
    }

    private static void PrintUsageLine(string programName, IDictionary<string, OptionConfiguration> optionsConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations) {
        var options = optionsConfigurations.Any() ? "[OPTIONS] " : string.Empty;
        var commands = commandConfigurations.Any() ? "[COMMAND] " : string.Empty;
        var arguments = true ? "[ARGUMENTS]" : string.Empty; // enabled by default
        var usageLie = $"Usage: {programName} {options}{commands}{arguments}";
        Console.WriteLine(usageLie);
    }

    private static void PrintProgramDescription() {
        Console.WriteLine("_____________________________________________________"); //program description
    }

    private void PrintOptions(IDictionary<string, OptionConfiguration> optionsConfigurations) {
        Console.WriteLine("Options:");
        foreach (var optionConfiguration in optionsConfigurations.Values) {
            var shortOptionName = ShortOptionName(optionConfiguration.PrimaryName);
            var comma = Comma(optionConfiguration.PrimaryName, optionConfiguration.SecondaryName);
            var longOptionName = LongOptionName(optionConfiguration.SecondaryName);
            var optionArgument = OptionArgument(optionConfiguration.Argument);
            var optionLine = $"  {shortOptionName}{comma} {longOptionName} {optionArgument}";
            if (optionLine.Length <= 27) {
                var optionLineWithFirstColumnWithPadding = optionLine.PadRight(27, ' ');
                var optionLineWithSecondColumnWithPadding =
                    optionLineWithFirstColumnWithPadding.PadRight(80, '_'); //option description
                Console.WriteLine(optionLineWithSecondColumnWithPadding);
            }
            else {
                Console.WriteLine(optionLine);
            }
        }
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

    private static void PrintCommands(IDictionary<string, CommandConfiguration> commandConfigurations) {
        Console.WriteLine("Commands:");
        foreach (var commandDefinition in commandConfigurations) {
            var command = commandDefinition.Key;
            var commandLine = $"  {command}";
            var commandLineWithFirstColumnWithPadding = commandLine.PadRight(14, ' ');
            var commandLineWithSecondColumnWithPadding =
                commandLineWithFirstColumnWithPadding.PadRight(80, '_'); //selectedCommand description
            Console.WriteLine(commandLineWithSecondColumnWithPadding);
        }
        
    }
}