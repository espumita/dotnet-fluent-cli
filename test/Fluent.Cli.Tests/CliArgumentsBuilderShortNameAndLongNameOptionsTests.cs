using System;
using Bogus;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliArgumentsBuilderShortNameAndLongNameOptionsTests {
    private Faker faker;
    private OptionFaker anOption;

    [SetUp]
    public void SetUp() {
        faker = new Faker();
        anOption = new OptionFaker(faker);
    }

    [Test]
    public void throw_argument_exception_when_option_short_name_is_not_correctly_configured(){
        var anOptionShortName = anOption.ShortNameWithNonAlphanumericalValue();
        var anOptionLongName = anOption.LongNameWithNonAlphanumericalValue();
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName, anOptionLongName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"'{anOptionShortName}' is not a valid short option, only alpha-numeric chars (a-zA-Z0-1) values can be configured");
    }

    [Test]
    public void throw_argument_exception_when_option_long_name_is_not_correctly_configured() {
        var anOptionShortName = anOption.ShortName();
        var anOptionLongName = anOption.LongNameWithNonAlphanumericalValue();
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName, anOptionLongName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"'{anOptionLongName}' is not a valid option, only alpha-numeric values and words separated by hyphen minus '-' can be configured");
    }

    [Test]
    public void throw_argument_exception_when_option_long_name_is_null() {
        var anOptionShortName = anOption.ShortName();
        var environmentArgs = new string[] { };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName, null)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"Option long name cannot be null, use other method instead");
    }

    [Test]
    public void mark_option_as_as_not_present() {
        var anOptionShortName = anOption.ShortName();
        var anOptionLongName = anOption.LongName();
        var environmentArgs = new string[] { };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName, anOptionLongName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Option(anOptionShortName).IsPresent.Should().BeFalse();
        cli.Option(anOptionLongName).IsPresent.Should().BeFalse();
    }

    [Test]
    public void mark_option_as_present_when_short_name_is_present() {
        var anOptionShortName = anOption.ShortName();
        var shortNamePrefix = anOption.ShortNamePrefix();
        var anOptionLongName = anOption.LongName();
        var environmentArgs = new [] { $"{shortNamePrefix}{anOptionShortName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName, anOptionLongName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Option(anOptionShortName).IsPresent.Should().BeTrue();
        cli.Option(anOptionLongName).IsPresent.Should().BeTrue();
    }

    [Test]
    public void mark_option_as_present_when_long_name_is_present() {
        var anOptionShortName = anOption.ShortName();
        var anOptionLongName = anOption.LongName();
        var longNamePrefix = anOption.LongNamePrefix();
        var environmentArgs = new[] { $"{longNamePrefix}{anOptionLongName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName, anOptionLongName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Option(anOptionShortName).IsPresent.Should().BeTrue();
        cli.Option(anOptionLongName).IsPresent.Should().BeTrue();
    }

    [Test]
    public void mark_option_as_present_when_short_name_and_long_name_are_present() {
        var anOptionShortName = anOption.ShortName();
        var anOptionLongName = anOption.LongName();
        var shortNamePrefix = anOption.ShortNamePrefix();
        var longNamePrefix = anOption.LongNamePrefix();
        var environmentArgs = new[] { $"{shortNamePrefix}{anOptionShortName}", $"{longNamePrefix}{anOptionLongName}" };

        var cli = CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName, anOptionLongName)
            .Build();

        cli.Options.Count.Should().Be(1);
        cli.Option(anOptionShortName).IsPresent.Should().BeTrue();
        cli.Option(anOptionLongName).IsPresent.Should().BeTrue();
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }
}