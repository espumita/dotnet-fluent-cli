using Fluent.Cli.Parsers;

namespace Fluent.Cli;

public class OptionsArgumentsParser {
    private readonly LongOptionsWithArgumentParser _longOptionsWithArgumentParser;
    private readonly ShortOptionsWithArgumentParser _shortOptionsWithArgumentParser;
    private readonly LongOptionsParser _longOptionsParser;
    private readonly ShortOptionsParser _shortOptionsParser;
    private readonly MultipleShortOptionsParser _multipleShortOptionsParser;
    private readonly UndefinedOptionsParser _undefinedOptionsParser;

    public OptionsArgumentsParser(
        LongOptionsWithArgumentParser longOptionsWithArgumentParser,
            ShortOptionsWithArgumentParser shortOptionsWithArgumentParser,
            LongOptionsParser longOptionsParser,
            ShortOptionsParser shortOptionsParser,
            MultipleShortOptionsParser multipleShortOptionsParser,
            UndefinedOptionsParser undefinedOptionsParser) {
        _longOptionsWithArgumentParser = longOptionsWithArgumentParser;
        _shortOptionsWithArgumentParser = shortOptionsWithArgumentParser;
        _longOptionsParser = longOptionsParser;
        _shortOptionsParser = shortOptionsParser;
        _multipleShortOptionsParser = multipleShortOptionsParser;
        _undefinedOptionsParser = undefinedOptionsParser;
    }

    public OptionsArgumentsParserResult ParseFrom(IList<string> environmentArgs) {
        var parserResult = new OptionsArgumentsParserResult();
        foreach (var argument in environmentArgs) {
            var optionsParser = OptionsParserFor(argument);
           // if (optionsParser == null) continue; //TODO
            var presentOptions = optionsParser.TryToParse(argument);
            parserResult.Add(presentOptions);
        }
        return parserResult;
    }

    private IOptionsParser OptionsParserFor(string argument) {
        if (_longOptionsWithArgumentParser.IsALongOptionWithArgument(argument)) return _longOptionsWithArgumentParser;
        if (_shortOptionsWithArgumentParser.IsAnOptionWithArgument(argument)) return _shortOptionsWithArgumentParser;
        if (_longOptionsParser.IsALongOption(argument)) return _longOptionsParser;
        if (_shortOptionsParser.IsAShortOption(argument)) return _shortOptionsParser;
        if (_multipleShortOptionsParser.AreMultipleShortOptions(argument)) return _multipleShortOptionsParser;
        if (_undefinedOptionsParser.IsAnUndefinedOption(argument)) return _undefinedOptionsParser;
        return null;
    }

}