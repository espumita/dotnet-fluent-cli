using Fluent.Cli.Parsers;

namespace Fluent.Cli;

public class CliArgumentsParser {
    private readonly IDictionary<string, Option> _optionsMap;
    private readonly LongOptionsWithArgumentOptionsParser _longOptionsWithArgumentOptionsParser;
    private readonly ShortOptionsWithArgumentOptionsParser _shortOptionsWithArgumentOptionsParser;
    private readonly LongOptionsArgumentOptionsParser _longOptionsArgumentOptionsParser;
    private readonly ShortOptionsArgumentOptionsParser _shortOptionsArgumentOptionsParser;
    private readonly MultipleShortOptionsArgumentOptionsParser _multipleShortOptionsArgumentOptionsParser;
    private readonly UndefinedOptionsArgumentOptionsParser _undefinedOptionsArgumentOptionsParser;

    public CliArgumentsParser(
            IDictionary<string, Option> optionsMap,
            LongOptionsWithArgumentOptionsParser longOptionsWithArgumentOptionsParser,
            ShortOptionsWithArgumentOptionsParser shortOptionsWithArgumentOptionsParser,
            LongOptionsArgumentOptionsParser longOptionsArgumentOptionsParser,
            ShortOptionsArgumentOptionsParser shortOptionsArgumentOptionsParser,
            MultipleShortOptionsArgumentOptionsParser multipleShortOptionsArgumentOptionsParser,
            UndefinedOptionsArgumentOptionsParser undefinedOptionsArgumentOptionsParser) {
        _optionsMap = optionsMap;
        this._longOptionsWithArgumentOptionsParser = longOptionsWithArgumentOptionsParser;
        this._shortOptionsWithArgumentOptionsParser = shortOptionsWithArgumentOptionsParser;
        this._longOptionsArgumentOptionsParser = longOptionsArgumentOptionsParser;
        this._shortOptionsArgumentOptionsParser = shortOptionsArgumentOptionsParser;
        this._multipleShortOptionsArgumentOptionsParser = multipleShortOptionsArgumentOptionsParser;
        this._undefinedOptionsArgumentOptionsParser = undefinedOptionsArgumentOptionsParser;
    }

    public CliArguments ParseFrom(string[] environmentArgs) {
        foreach (var arg in environmentArgs) {
            var optionsParser = OptionsParserFor(arg);
            var options = optionsParser.TryToParse(arg);
            foreach (var option in options) {
                _optionsMap[option.key] = option.NewOption;
            }
        }
        return new CliArguments(
            options: _optionsMap.Values.ToList()
        );
    }

    private IOptionsParser OptionsParserFor(string arg) {
        if (_longOptionsWithArgumentOptionsParser.IsALongOptionWithArgument(arg)) return _longOptionsWithArgumentOptionsParser;
        if (_shortOptionsWithArgumentOptionsParser.IsAnOptionWithArgument(arg)) return _shortOptionsWithArgumentOptionsParser;
        if (_longOptionsArgumentOptionsParser.IsALongOption(arg)) return _longOptionsArgumentOptionsParser;
        if (_shortOptionsArgumentOptionsParser.IsAShortOption(arg)) return _shortOptionsArgumentOptionsParser;
        if (_multipleShortOptionsArgumentOptionsParser.AreMultipleShortOptions(arg)) return _multipleShortOptionsArgumentOptionsParser;
        if (_undefinedOptionsArgumentOptionsParser.IsAnUndefinedOption(arg)) return _undefinedOptionsArgumentOptionsParser;
        return null;
    }



}