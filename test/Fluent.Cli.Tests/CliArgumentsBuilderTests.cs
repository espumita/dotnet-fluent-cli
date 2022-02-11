using System;
using System.Linq;
using Bogus;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliArgumentsBuilderTests {
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
    [TestCase("a--")]
    [TestCase("--a")]
    [TestCase("-a")]
    [TestCase("a-")]
    [TestCase("a-a-")]
    [TestCase("-a-a")]
    public void throw_argument_exception_when_option_short_name_is_not_correctly_configured(string optionShortName) {
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(shortName: optionShortName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"{optionShortName} is not a valid option, only alpha-numeric values can be configured");
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
    [TestCase("a--")]
    [TestCase("--a")]
    [TestCase("-a")]
    [TestCase("a-")]
    [TestCase("a-a-")]
    [TestCase("-a-a")]
    public void throw_argument_exception_when_option_name_is_not_correctly_configured(string optionName) {
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(shortName: "r", name: optionName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"{optionName} is not a valid option, only alpha-numeric values can be configured");
    }

    [Test]
    public void throw_option_is_not_configured_exception_when_try_to_get_option_not_configured() {
        var anOptionShortName = AnOptionNameWith(length: AnOptionLength());
        var environmentArgs = new string[] { };
        var cli = CliBuilderFrom(environmentArgs)
            .Build();

        Action action = () => cli.Option(anOptionShortName);

        action.Should().Throw<OptionIsNotConfiguredException>()
            .And.Message.Should().Be($"Option -- '{anOptionShortName}' has not been configured yet, add it to the builder first.");
    }

    [Test]
    public void mark_option_with_short_name_as_not_present() {
        var anOptionShortName = AnOptionNameWith(length: AnOptionLength());
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeFalse();
    }

    [Test]
    public void mark_option_with_name_as_not_present() {
        var anOptionShortName = AnOptionNameWith(length: 1);
        var anOptionName = AnOptionNameWith(length: 1);
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName, name: anOptionName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeFalse();
        cli.Option(anOptionShortName).IsPresent.Should().BeFalse();
        cli.Option(anOptionName).IsPresent.Should().BeFalse();
    }

    [TestCase("-")]
    [TestCase("--")]
    [TestCase("/")]
    public void do_not_read_options_when_they_are_not_configured(string validOptionPrefix) {
        var anOptionShortName = AnOptionNameWith(length: AnOptionLength());
        var environmentArgs = new [] { $"{validOptionPrefix}{anOptionShortName}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{anOptionShortName[0]}'\r\nTry 'PROGRAM --help' for more information.");
    }

    [TestCase("-")]
    [TestCase("--")]
    [TestCase("/")]
    public void mark_option_with_short_name_as_present(string validOptionPrefix) {
        var anOptionShortName = AnOptionNameWith(length: AnOptionLength());
        var environmentArgs = new [] { $"{validOptionPrefix}{anOptionShortName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
    }

    [TestCase("-", "a-a")]
    [TestCase("--", "a-a")]
    [TestCase("/", "a-a")]
    [TestCase("-", "a-a-a")]
    [TestCase("--", "a-a-a")]
    [TestCase("/", "a-a-a")]
    [TestCase("-", "a--a--a")]
    [TestCase("--", "a--a--a")]
    [TestCase("/", "a--a--a")]
    public void mark_option_with_hyphen_minus_configured_in_the_short_name_as_present(string validOptionPrefix, string optionName) {
        var environmentArgs = new[] { $"{validOptionPrefix}{optionName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: optionName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
        cli.Option(optionName).IsPresent.Should().BeTrue();
    }

    [TestCase("-", "r")]
    [TestCase("--", "r")]
    [TestCase("/", "r")]
    [TestCase("-", "rrr")]
    [TestCase("--","rrr")]
    [TestCase("/","rrr")]
    public void mark_simple_character_options_repeated_as_present(string validOptionPrefix, string args) {
        var environmentArgs = new[] { $"{validOptionPrefix}{args}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: "r")
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
    }

    [TestCase("-", "arr")]
    [TestCase("--", "rar")]
    [TestCase("/", "rra")]
    public void do_not_read_simple_character_options_repeated_when_they_are_not_configured(string validOptionPrefix, string args) {
        var environmentArgs = new[] { $"{validOptionPrefix}{args}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(shortName: "r")
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- 'a'\r\nTry 'PROGRAM --help' for more information.");
    }

    [TestCase("-", "arr")]
    [TestCase("--", "rar")]
    [TestCase("/", "rra")]
    public void mark_multiple_simple_character_options_repeated_as_present(string validOptionPrefix, string args) {
        var environmentArgs = new[] { $"{validOptionPrefix}{args}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: "r")
            .Option(shortName: "a")
            .Build();

        cli.Options.Count.Should().Be(2);
        cli.Option("r").IsPresent.Should().BeTrue();
        cli.Option("a").IsPresent.Should().BeTrue();
    }

    [TestCase("-")]
    [TestCase("--")]
    [TestCase("/")]
    public void mark_option_with_name_as_present(string validOptionPrefix) {
        var anOptionShortName = AnOptionNameWith(length: AnOptionLength());
        var anOptionName = AnOptionNameWith(length: AnOptionLength());
        var environmentArgs = new[] { $"{validOptionPrefix}{anOptionName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName, name: anOptionName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
        cli.Option(anOptionShortName).IsPresent.Should().BeTrue();
        cli.Option(anOptionName).IsPresent.Should().BeTrue();
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }

    private short AnOptionLength() {
        return faker.Random.Short(0, 100);
    }

    private string AnOptionNameWith(int length) {
        return faker.Random.AlphaNumeric(length);
    }

}