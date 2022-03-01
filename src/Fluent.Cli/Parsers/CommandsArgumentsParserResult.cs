namespace Fluent.Cli.Parsers;

public class CommandsArgumentsParserResult {
    public ArgumentCommand Command { get; set; }

    public void AddCommand(ArgumentCommand command) {
        Command = command;
    }
}