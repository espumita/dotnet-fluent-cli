using System.Text.RegularExpressions;

namespace Fluent.Cli.Preprocess;

public class ArgumentsPreprocessor {
    private readonly bool _enableCommandProcess;
    private readonly bool _enableOptionsProcess;
    private readonly bool _enableArgumentProcess;

    public ArgumentsPreprocessor(bool enableCommandProcess, bool enableOptionsProcess, bool enableArgumentProcess) {
        _enableCommandProcess = enableCommandProcess;
        _enableOptionsProcess = enableOptionsProcess;
        _enableArgumentProcess = enableArgumentProcess;
    }

    public ArgumentsPreprocessResult Preprocess(string[] environmentArgs, CommandsDefinitions commandsDefinitions) {
        var argumentsPreprocessResult = new ArgumentsPreprocessResult();
        var argumentsQueue = new Queue<string>(environmentArgs);
        var executableFileName = ExecutableFileName(argumentsQueue);
        argumentsPreprocessResult.AddProgramName(executableFileName);

        while (argumentsQueue.Any()) {
            var currentArgument = argumentsQueue.Dequeue();
            if (_enableCommandProcess && ThereIsNoCommandYetDetected(argumentsPreprocessResult) && IsPossibleCommand(currentArgument) && IsConfiguredCommand(currentArgument, commandsDefinitions)) {
                var possibleCommand = new PossibleCommand(currentArgument);
                argumentsPreprocessResult.AddPossibleCommand(possibleCommand);
            } else if (_enableOptionsProcess && IsAPossibleOption(currentArgument)) {
                argumentsPreprocessResult.AddPossibleOption(currentArgument);
            } else if (_enableArgumentProcess && IsPossibleArgument(currentArgument)) {
                argumentsPreprocessResult.AddPossibleArgument(currentArgument);
            }
        }
        return argumentsPreprocessResult;
    }

    private static string ExecutableFileName(Queue<string> argumentsQueue) {
        var environmentCommandLineArgs = Environment.GetCommandLineArgs();
        var executableFileName = environmentCommandLineArgs[0];
        if (argumentsQueue.Any()) {
            var peek = argumentsQueue.Peek();
            if (peek.Equals(executableFileName)) return argumentsQueue.Dequeue();
        }
        return executableFileName;
    }

    private static bool ThereIsNoCommandYetDetected(ArgumentsPreprocessResult argumentsPreprocessResult) {
        return argumentsPreprocessResult.PossibleCommand == null;
    }

    private static bool IsPossibleCommand(string currentArgument) {
        return currentArgument.Length > 0 && Regex.IsMatch(currentArgument, "^[a-zA-Z0-9]+$");
    }

    private static bool IsConfiguredCommand(string currentArgument, CommandsDefinitions commandsDefinitions) {
        return commandsDefinitions.IsCommandDefined(currentArgument);
    }

    private static bool IsAPossibleOption(string currentArgument) {
        if (string.IsNullOrEmpty(currentArgument) || currentArgument.Length <= 1) return false;
        return Regex.IsMatch(currentArgument, "^(-)(.*)");
    }

    private static bool IsPossibleArgument(string environmentArg) {
        return environmentArg.Length > 0;
    }
}