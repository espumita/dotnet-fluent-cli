namespace Fluent.Cli.ConsolePrinters; 

public class VersionOptionConsolePrinter {

    public void PrintVersionAndStopProcess(string programName, string programVersion) {
        Console.Write($"{programName} version {programVersion}");
        Environment.Exit(0);
    }
}