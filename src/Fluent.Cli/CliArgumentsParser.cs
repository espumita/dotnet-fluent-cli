using Fluent.Cli.Parsers;

namespace Fluent.Cli;

public class CliArgumentsParser {
    private readonly LongOptionsWithArgumentParser _longOptionsWithArgumentParser;
    private readonly ShortOptionsWithArgumentParser _shortOptionsWithArgumentParser;
    private readonly LongOptionsParser _longOptionsParser;
    private readonly ShortOptionsParser _shortOptionsParser;
    private readonly MultipleShortOptionsParser _multipleShortOptionsParser;
    private readonly UndefinedOptionsParser _undefinedOptionsParser;

    public CliArgumentsParser(
        LongOptionsWithArgumentParser longOptionsWithArgumentParser,
            ShortOptionsWithArgumentParser shortOptionsWithArgumentParser,
            LongOptionsParser longOptionsParser,
            ShortOptionsParser shortOptionsParser,
            MultipleShortOptionsParser multipleShortOptionsParser,
            UndefinedOptionsParser undefinedOptionsParser) {
        this._longOptionsWithArgumentParser = longOptionsWithArgumentParser;
        this._shortOptionsWithArgumentParser = shortOptionsWithArgumentParser;
        this._longOptionsParser = longOptionsParser;
        this._shortOptionsParser = shortOptionsParser;
        this._multipleShortOptionsParser = multipleShortOptionsParser;
        this._undefinedOptionsParser = undefinedOptionsParser;
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
        if (_longOptionsWithArgumentParser.IsALongOptionWithArgument(argument)) return _longOptionsWithArgumentParser;
        if (_shortOptionsWithArgumentParser.IsAnOptionWithArgument(argument)) return _shortOptionsWithArgumentParser;
        if (_longOptionsParser.IsALongOption(argument)) return _longOptionsParser;
        if (_shortOptionsParser.IsAShortOption(argument)) return _shortOptionsParser;
        if (_multipleShortOptionsParser.AreMultipleShortOptions(argument)) return _multipleShortOptionsParser;
        if (_undefinedOptionsParser.IsAnUndefinedOption(argument)) return _undefinedOptionsParser;
        return null;
    }



}