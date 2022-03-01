using Fluent.Cli.Options;

namespace Fluent.Cli;

public class OptionsArgumentsParserResult {
    public readonly List<ArgumentOption> presentOptions;
    public OptionsArgumentsParserResult() {
        presentOptions = new List<ArgumentOption>();
    }

    public void Add(IList<ArgumentOption> options) {
        presentOptions.AddRange(options);
    }
}