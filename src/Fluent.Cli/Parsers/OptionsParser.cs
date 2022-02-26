using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public interface IOptionsParser {
    IList<ArgumentOption> TryToParse(string argument);
}