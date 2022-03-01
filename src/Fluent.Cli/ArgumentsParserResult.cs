namespace Fluent.Cli;

public class ArgumentsParserResult {
    public readonly List<string> arguments;
    public ArgumentsParserResult() {
        arguments = new List<string>();
    }

    public void Add(string argument) {
        arguments.Add(argument);
    }
}