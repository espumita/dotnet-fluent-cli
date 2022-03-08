namespace Fluent.Cli.ConsolePrinters; 

public class VersionOptionConsolePrinter {
    private readonly string _programName;
    private readonly string _programVersion;

    public VersionOptionConsolePrinter(string programName, string programVersion) {
        _programName = programName;
        _programVersion = programVersion;
    }

    public void PrintVersionAndStopProcess() {
        Console.Write($"{_programName} version {_programVersion}");
        Environment.Exit(0);
    }
}