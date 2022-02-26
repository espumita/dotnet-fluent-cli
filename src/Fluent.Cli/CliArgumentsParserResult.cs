using Fluent.Cli.Options;

namespace Fluent.Cli;

public class CliArgumentsParserResult {
    public readonly List<ArgumentOption> presentOptions;
    public CliArgumentsParserResult() {
        presentOptions = new List<ArgumentOption>();
    }

    public void Add(IList<ArgumentOption> options) {
        presentOptions.AddRange(options);
    }
}