using Fluent.Cli.Preprocess;

namespace Fluent.Cli.Parsers;

public class CommandArgumentsParser {
    private readonly CommandsDefinitions _commandDefinitions;

    public CommandArgumentsParser(CommandsDefinitions commandDefinitions) {
        _commandDefinitions = commandDefinitions;
    }

    public CommandsArgumentsParserResult ParseFrom(PossibleCommand? possibleCommand) {
        var commandsArgumentsParserResult = new CommandsArgumentsParserResult();
        if (possibleCommand == null) return commandsArgumentsParserResult;
        var commandDefinition = _commandDefinitions.GetCommand(possibleCommand.Name);
        //TODO check for arguments and options
        commandsArgumentsParserResult.AddCommand(CommandWith(possibleCommand.Name));
        return commandsArgumentsParserResult;
    }

    private static ArgumentCommand CommandWith(string commandName) {
        return new ArgumentCommand(commandName);
    }
}