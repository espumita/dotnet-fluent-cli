namespace Fluent.Cli;

public class ArgumentsPreprocessResult {

    public string Program { get; set; }
    public IList<string> PossibleArguments { get; set; }
    public IList<string> PossibleOptions { get; set; }

    public ArgumentsPreprocessResult() {
        PossibleArguments = new List<string>();
        PossibleOptions = new List<string>();
    }

    public void AddProgramName(string programName) {
        Program = programName;
    }

    public void AddPossibleOption(string argument) {
        PossibleOptions.Add(argument);
    }

    public void AddPossibleArgument(string argument) {
        PossibleArguments.Add(argument);
    }
}