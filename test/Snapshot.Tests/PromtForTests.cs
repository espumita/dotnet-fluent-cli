using System.Diagnostics;
using System.Threading.Tasks;

namespace Snapshot.Tests;

public class PromtForTests {
    private readonly string executableFileName;

    public PromtForTests(string executableFileName) {
        this.executableFileName = executableFileName;
    }

    public async Task<string> RunWithArgumentsAndGetOutput(string args) {
        var process = ProcessStartInfo(args);

        using (var executedProcess = Process.Start(process)) {
            return await executedProcess.StandardOutput.ReadToEndAsync();
        }
    }

    private ProcessStartInfo ProcessStartInfo(string args) {
        return new ProcessStartInfo {
            CreateNoWindow = false,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = executableFileName,
            WindowStyle = ProcessWindowStyle.Hidden,
            Arguments = args
        };
    }
}