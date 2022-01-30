using System.Linq;
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

    [Test]
    public void do_not_read_flags_that_are_not_configured() {
        var aFlagOption = AFlagOption()
            .BuildWithTestValues();
        var environmentArgs = new [] { aFlagOption.ShortName };

        var args = CliArgsBuilderFrom(environmentArgs)
            .Build();

        args.Flags.Should().BeEmpty();
    }

    [Test]
    public void mark_flag_as_not_present() {
        var aFlagOption = AFlagOption()
            .BuildWithTestValues();
        var environmentArgs = new string[] { };

        var args = CliArgsBuilderFrom(environmentArgs)
            .AddFlag(config => config.ShortName = aFlagOption.ShortName)
            .Build();

        args.Flags.Count.Should().Be(1);
        args.Flags.Single().IsPresent.Should().BeFalse();
    }

    [Test]
    public void mark_flag_as_present() {
        var aFlagOption = AFlagOption()
            .BuildWithTestValues();

        var environmentArgs = new [] { aFlagOption.ShortName };

        var args = CliArgsBuilderFrom(environmentArgs)
            .AddFlag(config => config.ShortName = aFlagOption.ShortName)
            .Build();

        args.Flags.Count.Should().Be(1);
        args.Flags.Single().IsPresent.Should().BeTrue();
    }

    [TestCase("f", "f")]
    [TestCase("-f", "f")]
    [TestCase("force", "force")]
    [TestCase("-force", "force")]
    [TestCase("--force", "force")]
    public void mark_flag_as_present_in_different_formats(string environmentArg, string flagShortName) {
        var aFlagOption = AFlagOption()
            .BuildWithTestValues();

        var environmentArgs = new[] { environmentArg };

        var args = CliArgsBuilderFrom(environmentArgs)
            .AddFlag(config => config.ShortName = flagShortName)
            .Build();

        args.Flags.Count.Should().Be(1);
        args.Flags.Single().IsPresent.Should().BeTrue();
    }

    private static CliArgsBuilder CliArgsBuilderFrom(string[] args) {
        return Args.CliArgsBuilder.From(args);
    }

    private FlagOptionBuilder AFlagOption() {
        return new FlagOptionBuilder();
    }
}