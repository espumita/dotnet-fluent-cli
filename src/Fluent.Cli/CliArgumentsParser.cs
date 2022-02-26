using Fluent.Cli.Parsers;

namespace Fluent.Cli;

public class CliArgumentsParser {
    private readonly LongOptionsWithArgumentOptionsParser _longOptionsWithArgumentOptionsParser;
    private readonly ShortOptionsWithArgumentOptionsParser _shortOptionsWithArgumentOptionsParser;
    private readonly LongOptionsArgumentOptionsParser _longOptionsArgumentOptionsParser;
    private readonly ShortOptionsArgumentOptionsParser _shortOptionsArgumentOptionsParser;
    private readonly MultipleShortOptionsArgumentOptionsParser _multipleShortOptionsArgumentOptionsParser;
    private readonly UndefinedOptionsArgumentOptionsParser _undefinedOptionsArgumentOptionsParser;

    public CliArgumentsParser(
        LongOptionsWithArgumentOptionsParser longOptionsWithArgumentOptionsParser,
            ShortOptionsWithArgumentOptionsParser shortOptionsWithArgumentOptionsParser,
            LongOptionsArgumentOptionsParser longOptionsArgumentOptionsParser,
            ShortOptionsArgumentOptionsParser shortOptionsArgumentOptionsParser,
            MultipleShortOptionsArgumentOptionsParser multipleShortOptionsArgumentOptionsParser,
            UndefinedOptionsArgumentOptionsParser undefinedOptionsArgumentOptionsParser) {
        this._longOptionsWithArgumentOptionsParser = longOptionsWithArgumentOptionsParser;
        this._shortOptionsWithArgumentOptionsParser = shortOptionsWithArgumentOptionsParser;
        this._longOptionsArgumentOptionsParser = longOptionsArgumentOptionsParser;
        this._shortOptionsArgumentOptionsParser = shortOptionsArgumentOptionsParser;
        this._multipleShortOptionsArgumentOptionsParser = multipleShortOptionsArgumentOptionsParser;
        this._undefinedOptionsArgumentOptionsParser = undefinedOptionsArgumentOptionsParser;
    }

    public CliArgumentsParserResult ParseFrom(string[] environmentArgs) {
        var parserResult = new CliArgumentsParserResult();
        foreach (var argument in environmentArgs) {
            var optionsParser = OptionsParserFor(argument);
            if (optionsParser == null) continue; //TODO
            var presentOptions = optionsParser.TryToParse(argument);
            parserResult.Add(presentOptions);
        }
        return parserResult;
    }

    private IOptionsParser OptionsParserFor(string argument) {
        if (_longOptionsWithArgumentOptionsParser.IsALongOptionWithArgument(argument)) return _longOptionsWithArgumentOptionsParser;
        if (_shortOptionsWithArgumentOptionsParser.IsAnOptionWithArgument(argument)) return _shortOptionsWithArgumentOptionsParser;
        if (_longOptionsArgumentOptionsParser.IsALongOption(argument)) return _longOptionsArgumentOptionsParser;
        if (_shortOptionsArgumentOptionsParser.IsAShortOption(argument)) return _shortOptionsArgumentOptionsParser;
        if (_multipleShortOptionsArgumentOptionsParser.AreMultipleShortOptions(argument)) return _multipleShortOptionsArgumentOptionsParser;
        if (_undefinedOptionsArgumentOptionsParser.IsAnUndefinedOption(argument)) return _undefinedOptionsArgumentOptionsParser;
        return null;
    }



}