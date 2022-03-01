namespace Fluent.Cli.Preprocess;

public class ArgumentsPreprocessResult {

    public PossibleCommand? PossibleCommand { get; set; }
    public IList<string> PossibleOptions { get; set; }
    public IList<string> PossibleArguments { get; set; }

    public ArgumentsPreprocessResult() {
        PossibleCommand = null;
        PossibleOptions = new List<string>();
        PossibleArguments = new List<string>();

    }

    public void AddPossibleCommand(PossibleCommand possibleCommand) {
        PossibleCommand = possibleCommand;
    }

    public void AddPossibleOption(string argument) {
        PossibleOptions.Add(argument);
    }

    public void AddPossibleArgument(string argument) {
        PossibleArguments.Add(argument);
    }
}
