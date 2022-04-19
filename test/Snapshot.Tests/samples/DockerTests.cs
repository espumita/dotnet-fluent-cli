using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Snapshot.Tests.samples; 

public class DockerTests {
    private PromtForTests promt;
    private SnapshotReader snapshotReader;
    private const string executable = "docker.exe";
    private const string snapshotFileName = "docker_help_snapshot.txt";


    [SetUp]
    public void Setup() {
        promt = new PromtForTests(executable);
        snapshotReader = new SnapshotReader();
    }

    [Test]
    public async Task docker_help() {
        var environmentArgs = @"--help";

        var consoleOutput = await promt.RunWithArgumentsAndGetOutput(environmentArgs);

        var snapshotValue = await snapshotReader.Read(snapshotFileName);
        consoleOutput.Should().Be(snapshotValue);
    }

}