namespace Fluent.Cli.Parsers;

public class ArgumentsParser {
    public ArgumentsParserResult ParseFrom(IList<string> arguments) {
        var argumentsParserResult = new ArgumentsParserResult();
        foreach (var argument in arguments) {
            argumentsParserResult.Add(argument);
        }
        return argumentsParserResult;
    }

}