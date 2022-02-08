using System;
using System.Linq;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliBuilderTests {
    private Faker faker;

    [SetUp]
    public void SetUp() {
        faker = new Faker();
    }

    [Test]
    public void throw_argument_exception_when_a_null_is_received() {
        Action action = () => CliBuilderFrom(null)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be("args cannot be null");
    }

    [Test]
    public void do_not_read_anything_when_there_is_no_arguments() {
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .Build();

        cli.Options.Should().BeEmpty();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("-")]
    [TestCase("!")]
    [TestCase("--")]
    [TestCase("/")]
    [TestCase("-")]
    [TestCase("🐛")]
    [TestCase("a ")]
    [TestCase(" a")]
    [TestCase("a_a")]
    [TestCase("1_")]
    [TestCase("_1")]
    [TestCase("1_1")]
    [TestCase("1 = 1")]
    public void throw_argument_exception_when_option_is_not_correctly_configured(string optionShortName) {
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(shortName: optionShortName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"{optionShortName} is not a valid option, only alpha-numeric values can be configured");
    }

    [Test]
    public void throw_option_is_not_configured_exception_when_try_to_get_option_not_configured() {
        var anOptionShortName = AnOptionShortNameWith(length: AnOptionLength());
        var environmentArgs = new string[] { };
        var cli = CliBuilderFrom(environmentArgs)
            .Build();

        Action action = () => cli.Option(anOptionShortName);

        action.Should().Throw<OptionIsNotConfiguredException>()
            .And.Message.Should().Be($"Option -- '{anOptionShortName}' has not been configured yet, add it to the builder first.");
    }

    [Test]
    public void mark_option_with_short_name_as_not_present() {
        var anOptionShortName = AnOptionShortNameWith(length: AnOptionLength());
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeFalse();
    }

    [TestCase("-")]
    [TestCase("--")]
    [TestCase("/")]
    public void do_not_read_options_when_they_are_not_configured(string validOptionPrefix) {
        var anOptionShortName = AnOptionShortNameWith(length: AnOptionLength());
        var environmentArgs = new [] { $"{validOptionPrefix}{anOptionShortName}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{anOptionShortName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    [TestCase("-")]
    [TestCase("--")]
    [TestCase("/")]
    public void mark_option_with_short_name_as_present(string validOptionPrefix) {
        var anOptionShortName = AnOptionShortNameWith(length: AnOptionLength());
        var environmentArgs = new [] { $"{validOptionPrefix}{anOptionShortName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
    }

    private static CliBuilder CliBuilderFrom(string[] args) {
        return CliBuilder.With(args);
    }

    private short AnOptionLength() {
        return faker.Random.Short(0, 100);
    }

    private string AnOptionShortNameWith(int length) {
        return faker.Random.AlphaNumeric(length);
    }
}