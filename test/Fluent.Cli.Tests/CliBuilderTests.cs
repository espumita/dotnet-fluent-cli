using System.Linq;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliBuilderTests {
    private Faker faker;

    private const string AvailableOptionsCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [SetUp]
    public void SetUp() {
        faker = new Faker();
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
        var anOptionShortName = AnOptionShortNameWith(length: 1);
        var environmentArgs = new [] { anOptionShortName };

        var cli = CliBuilderFrom(environmentArgs)
            .Build();

        cli.Options.Should().BeEmpty();
    }

    [Test]
    public void mark_option_with_short_name_as_not_present() {
        var anOptionShortName = AnOptionShortNameWith(length: 1);
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeFalse();
    }

    [Test]
    public void mark_option_with_short_name_as_present() {
        var anOptionShortName = AnOptionShortNameWith(length: 1);
        var environmentArgs = new [] { anOptionShortName };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
    }

    //[TestCase("f", "f")]
    //[TestCase("-f", "f")]
    //[TestCase("force", "force")]
    //[TestCase("-force", "force")]
    //[TestCase("--force", "force")]
    //public void mark_option_as_present_in_different_formats(string option, string shortName) {
    //    var environmentArgs = new[] { option };

    //    var cli = CliBuilderFrom(environmentArgs)
    //        .Option(shortName)
    //        .Build();

    //    cli.Options.Count.Should().Be(1);
    //    cli.Options.Single().IsPresent.Should().BeTrue();
    //}

    private static CliBuilder CliBuilderFrom(string[] args) {
        return CliBuilder.With(args);
    }

    private string AnOptionShortNameWith(int length) {
        return faker.Random.String2(1, length, AvailableOptionsCharacters);
    }
}