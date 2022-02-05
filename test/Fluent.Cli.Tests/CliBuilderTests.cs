using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliBuilderTests {


    [SetUp]
    public void SetUp() {

    }

    [Test]
    public void do_not_read_anything_when_there_is_no_arguments() {
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .Build();

        cli.Options.Should().BeEmpty();
    }

    [Test]
    public void do_not_read_options_when_they_are_not_configured() {
        var anOption = AnOption()
            .BuildWithTestValues();
        var environmentArgs = new [] { anOption.ShortName };

        var cli = CliBuilderFrom(environmentArgs)
            .Build();

        cli.Options.Should().BeEmpty();
    }

    [Test]
    public void mark_option_as_not_present() {
        var anOption = AnOption()
            .BuildWithTestValues();
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(config => config.ShortName = anOption.ShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeFalse();
    }

    [Test]
    public void mark_option_as_present() {
        var anOption = AnOption()
            .BuildWithTestValues();
        var environmentArgs = new [] { anOption.ShortName };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(config => config.ShortName = anOption.ShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
    }

    [Test]
    public void get_option_by_short_name() {
        var anOption = AnOption()
            .BuildWithTestValues();
        var environmentArgs = new[] { anOption.ShortName };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(config => config.ShortName = anOption.ShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Option(anOption.ShortName).IsPresent.Should().BeTrue();
    }

    [TestCase("f", "f")]
    [TestCase("-f", "f")]
    [TestCase("force", "force")]
    [TestCase("-force", "force")]
    [TestCase("--force", "force")]
    public void mark_option_as_present_in_different_formats(string option, string shortName) {
        var environmentArgs = new[] { option };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(config => config.ShortName = shortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
    }

    private static CliBuilder CliBuilderFrom(string[] args) {
        return CliBuilder.From(args);
    }

    private OptionBuilder AnOption() {
        return new OptionBuilder();
    }
}