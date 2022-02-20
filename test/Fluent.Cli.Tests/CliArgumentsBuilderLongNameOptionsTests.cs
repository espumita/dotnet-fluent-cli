using System;
using System.Linq;
using Bogus;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 
public class CliArgumentsBuilderLongNameOptionsTests {
    private Faker faker;
    private OptionFaker anOption;

    [SetUp]
    public void SetUp() {
        faker = new Faker();
        anOption = new OptionFaker(faker);
    }

    [Test]
    public void throw_argument_exception_when_option_long_name_is_not_correctly_configured() {
        var anOptionLongName = anOption.LongNameWithNonAlphanumericalValue();
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .LongOption(longName: anOptionLongName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"'{anOptionLongName}' is not a valid option, only alpha-numeric values and words separated by hyphen minus '-' can be configured");
    }

    [Test]
    public void throw_argument_exception_when_option_long_name_is_null() {
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .LongOption(null)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"Option long name cannot be null or empty, use other method instead");
    }

    [TestCase("-a-a")]
    [TestCase("a-a-")]
    [TestCase("--a-a")]
    [TestCase("-a-a--")]
    public void throw_argument_exception_when_option_long_name_is_not_correctly_configured(string optionLongName) {
        var anOptionLongName = anOption.LongNameWithNonAlphanumericalValue();
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .LongOption(longName: anOptionLongName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"'{anOptionLongName}' is not a valid option, only alpha-numeric values and words separated by hyphen minus '-' can be configured");
    }

    [Test]
    public void mark_option_with_long_name_as_not_present() {
        var anOptionLongName = anOption.LongName();
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .LongOption(longName: anOptionLongName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeFalse();
        cli.Option(anOptionLongName).IsPresent.Should().BeFalse();
    }

    [Test]
    public void mark_option_with_name_as_present() {
        var anOptionLongName = anOption.LongName();
        var longNamePrefix = anOption.LongNamePrefix();
        var environmentArgs = new[] { $"{longNamePrefix}{anOptionLongName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .LongOption(anOptionLongName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
        cli.Option(anOptionLongName).IsPresent.Should().BeTrue();
    }

    [Test]
    public void mark_multiple_option_with_long_name_as_present() {
        var anOptionLongName = anOption.LongName();
        var anotherOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}", $"{anOptionLongNamePrefix}{anotherOptionLongName}" };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .LongOption(anOptionLongName)
            .LongOption(anotherOptionLongName)
            .Build();

        cliArguments.Options.Count.Should().Be(2);
        cliArguments.Option(anOptionLongName).IsPresent.Should().BeTrue();
        cliArguments.Option(anotherOptionLongName).IsPresent.Should().BeTrue();
    }

    [TestCase("a-a")]
    [TestCase("a-a-a")]
    [TestCase("a--a--a")]
    public void mark_option_with_hyphen_minus_configured_in_the_short_name_as_present(string optionLongName) {
        var longNamePrefix = anOption.LongNamePrefix();
        var environmentArgs = new[] { $"{longNamePrefix}{optionLongName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .LongOption(optionLongName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Options.Single().IsPresent.Should().BeTrue();
        cli.Option(optionLongName).IsPresent.Should().BeTrue();
    }

    [TestCase("!")]
    [TestCase("Q-")]
    [TestCase("Q-Q")]
    [TestCase("Q-Q")]
    public void trow_exception_when_short_option_is_not_configured(string optionName) {
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{optionName}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }
    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }

}