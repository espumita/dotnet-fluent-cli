using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Dotnet.Cli.Args.Tests; 

public class CliArgsBuilderTests {

    private StringBuilder stringBuilder;

    [SetUp]
    public void SetUp() {
        stringBuilder = new StringBuilder();
    }


    [Test]
    public void do_not_read_any_flag() {
        var environmentArgs = new string[] { };

        var args = CliArgsBuilderFrom(environmentArgs)
            .Build();

        args.Flags.Should().BeEmpty();
    }

    private static CliArgsBuilder CliArgsBuilderFrom(string[] args) {
        return Args.CliArgsBuilder.From(args);
    }
}