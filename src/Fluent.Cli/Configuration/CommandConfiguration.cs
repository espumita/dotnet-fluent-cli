using System.Text.RegularExpressions;

namespace Fluent.Cli.Configuration;

public class CommandConfiguration {
    public string Name { get; }
    public string Description { get; private set; }

    private CommandConfiguration(string name) {
        Name = name;
    }

    public static CommandConfiguration For(string name) {
        Validate(name);
        return new CommandConfiguration(name);
    }

    public void AddDescription(string description) {
        Description = description;

    }

    private static void Validate(string name) {
        if (!Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
            throw new ArgumentException($"'{name}' is not a valid command, only alpha-numeric chars (a-zA-Z0-1) values can be configured");
    }
}