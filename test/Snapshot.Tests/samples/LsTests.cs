using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Snapshot.Tests.samples; 

public class LsTests {
    private PromtForTests promt;
    private SnapshotReader snapshotReader;
    private const string executable = "ls.exe";
    private const string snapshotFileName = "ls_help_snapshot.txt";


    [SetUp]
    public void Setup() {
        promt = new PromtForTests(executable);
        snapshotReader = new SnapshotReader();
    }

    [Test]
    public async Task ls_help() {
        var environmentArgs = @"--help";

        var consoleOutput = await promt.RunWithArgumentsAndGetOutput(environmentArgs);

        var snapshotValue = await snapshotReader.Read(snapshotFileName);
        consoleOutput.Should().Be(snapshotValue);
    }

}