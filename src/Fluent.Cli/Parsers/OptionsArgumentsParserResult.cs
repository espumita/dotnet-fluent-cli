using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class OptionsArgumentsParserResult {
    public readonly List<ArgumentOption> presentOptions;
    public OptionsArgumentsParserResult() {
        presentOptions = new List<ArgumentOption>();
    }

    public void Add(IList<ArgumentOption> options) {
        presentOptions.AddRange(options);
    }
}