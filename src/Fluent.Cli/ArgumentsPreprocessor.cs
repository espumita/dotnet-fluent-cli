using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class ArgumentsPreprocessor {
    private readonly bool _enableArgumentProcess;
    private readonly bool _enableOptionsProcess;

    public ArgumentsPreprocessor(bool enableArgumentProcess, bool enableOptionsProcess) {
        _enableArgumentProcess = enableArgumentProcess;
        _enableOptionsProcess = enableOptionsProcess;
    }

    public ArgumentsPreprocessResult Preprocess(string[] environmentArgs) {
        var argumentsPreprocessResult = new ArgumentsPreprocessResult();
        var argumentsQueue = new Queue<string>(environmentArgs);
        //TODO first argument should be considered program some times.

        while (argumentsQueue.Any()) {
            var currentArgument = argumentsQueue.Dequeue();
            if (_enableOptionsProcess && IsAPossibleOption(currentArgument)) {
                argumentsPreprocessResult.AddPossibleOption(currentArgument);
            } else if (_enableArgumentProcess && IsPossibleArgument(currentArgument)) {
                argumentsPreprocessResult.AddPossibleArgument(currentArgument);
            }
        }
        return argumentsPreprocessResult;
    }

    private static bool IsAPossibleOption(string currentArgument) {
        if (string.IsNullOrEmpty(currentArgument) || currentArgument.Length <= 1) return false;
        return Regex.IsMatch(currentArgument, "^(-)(.*)");
    }

    private static bool IsPossibleArgument(string environmentArg) {
        return environmentArg.Length > 0;
    }
}