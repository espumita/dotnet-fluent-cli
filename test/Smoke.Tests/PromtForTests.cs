using System.Diagnostics;
using System.Threading.Tasks;

namespace Smoke.Tests;

public class PromtForTests {
    private readonly string executableFileName;

    public PromtForTests(string executableFileName) {
        this.executableFileName = executableFileName;
    }

    public async Task<string> RunWithArguments(string args) {
        var process = new ProcessStartInfo {
            CreateNoWindow = false,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = executableFileName,
            WindowStyle = ProcessWindowStyle.Hidden,
            Arguments = args
        };

        using (var executedProcess = Process.Start(process)) {
            return await executedProcess.StandardOutput.ReadToEndAsync();
        }
    }
}