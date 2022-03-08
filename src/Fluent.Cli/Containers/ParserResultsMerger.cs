using Fluent.Cli.Configuration;
using Fluent.Cli.Options;
using Fluent.Cli.Parsers;

namespace Fluent.Cli.Containers; 

public class ParserResultsMerger {
    private readonly string _programName;
    private readonly string _programVersion;
    private readonly CommandsArgumentsParserResult _commandsArgumentsParserResult;
    private readonly OptionsArgumentsParserResult _optionsArgumentsParserResult;
    private readonly IDictionary<string, OptionConfiguration> _optionConfigurations;
    private readonly ArgumentsParserResult _argumentsParserResult;

    public ParserResultsMerger(string programName, string programVersion, CommandsArgumentsParserResult commandsArgumentsParserResult, OptionsArgumentsParserResult optionsArgumentsParserResult, IDictionary<string, OptionConfiguration> optionConfigurations, ArgumentsParserResult argumentsParserResult) {
        _programName = programName;
        _programVersion = programVersion;
        _commandsArgumentsParserResult = commandsArgumentsParserResult;
        _optionsArgumentsParserResult = optionsArgumentsParserResult;
        _optionConfigurations = optionConfigurations;
        _argumentsParserResult = argumentsParserResult;
    }

    public CliArguments MergeAsCliArguments() {
        var options = AllOptionsNotPresentByDefaultFrom(_optionConfigurations);
        var command = CommandFrom(_commandsArgumentsParserResult);
        MarkOptionsAsPresentBasedOn(_optionsArgumentsParserResult, options);
        var arguments = ArgumentsFrom(_argumentsParserResult);
        return new CliArguments(
            program: _programName,
            version: _programVersion,
            selectedCommand: command,
            options: options.Values.ToList(),
            arguments: arguments
        );
    }

    private List<Argument> ArgumentsFrom(ArgumentsParserResult argumentsParserResult) {
        return argumentsParserResult.arguments.Select(
            (argument, index) => new Argument($"${index}", argument)
        ).ToList();
    }

    private IDictionary<string, Option> AllOptionsNotPresentByDefaultFrom(IDictionary<string, OptionConfiguration> optionConfigurations) {
        return optionConfigurations
            .ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => OptionNotPresent(keyValuePair.Value)
            );
    }

    private Option OptionNotPresent(OptionConfiguration optionConfiguration) {
        return new Option(
            string.IsNullOrEmpty(optionConfiguration.PrimaryName) ? null : char.Parse(optionConfiguration.PrimaryName),
            optionConfiguration.SecondaryName,
            isPresent: false,
            argumentName: optionConfiguration.Argument?.ArgumentName
        );
    }

    private static Command? CommandFrom(CommandsArgumentsParserResult commandsArgumentsParserResult) {
        return commandsArgumentsParserResult.Command?.Name != null
            ? new Command(commandsArgumentsParserResult.Command.Name)
            : null;
    }

    private void MarkOptionsAsPresentBasedOn(OptionsArgumentsParserResult parserResult, IDictionary<string, Option> options) {
        parserResult.presentOptions.ForEach(presentOption => {
            var optionNamePresent = OptionNamePresent(presentOption.OptionNamePresent, options);
            var option = options[optionNamePresent];
            if (presentOption is ArgumentOptionWithArgument presentOptionWithArgument) {
                options[optionNamePresent] = OptionPresent(option, presentOptionWithArgument.ArgumentValue);
            } else {
                options[optionNamePresent] = OptionPresent(option);
            }
        });
    }

    private static string OptionNamePresent(string OptionName, IDictionary<string, Option> options) {
        return OptionName.Length == 1
            ? OptionName
            : options.First(keyValuePair => OptionName.Equals(keyValuePair.Value.Name)).Key;
    }

    private Option OptionPresent(Option option, string argumentValue = null) {
        return new Option(
            option.ShortName,
            option.Name,
            isPresent: true,
            argumentName: option.ArgumentPresent?.Name,
            argumentValue: argumentValue
        );
    }
}