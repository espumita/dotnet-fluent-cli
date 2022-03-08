namespace Fluent.Cli.Definitions;

public class CommandsDefinitions {
    public Dictionary<string, CommandDefinition> Definitions { get; set; }

    public bool IsCommandDefined(string commandName) {
        return Definitions.ContainsKey(commandName);
    }

    public CommandDefinition GetCommand(string commandName) {
        return Definitions[commandName];
    }
}