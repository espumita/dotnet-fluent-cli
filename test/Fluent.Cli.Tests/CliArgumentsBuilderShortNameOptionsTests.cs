using System;
using System.Linq;
using Bogus;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests;
public class CliArgumentsBuilderShortNameOptionsTests {
    private Faker faker;
    private OptionFaker anOption;

    [SetUp]
    public void SetUp() {
        faker = new Faker();
        anOption = new OptionFaker(faker);
    }

    [Test]
    public void throw_argument_exception_when_option_short_name_is_not_correctly_configured() {
        var anOptionShortName = anOption.ShortNameWithNonAlphanumericalValue();
        var environmentArgs = new string[] { };
        
        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"'{anOptionShortName}' is not a valid short option, only alpha-numeric chars (a-zA-Z0-1) values can be configured");
    }

    [Test]
    public void mark_option_with_short_name_as_not_present() {
        var anOptionShortName = anOption.ShortName();
        var environmentArgs = new string[] { };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        cliArguments.Options.Count.Should().Be(1);
        cliArguments.Options.Single().IsPresent.Should().BeFalse();
        cliArguments.Option(anOptionShortName).IsPresent.Should().BeFalse();
    }

    [Test]
    public void mark_option_with_short_name_as_present() {
        var anOptionShortName = anOption.ShortName();
        var anOptionShortNamePrefix = anOption.ShortNamePrefix();
        var environmentArgs = new[] { $"{anOptionShortNamePrefix}{anOptionShortName}" };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .Option(shortName: anOptionShortName)
            .Build();

        cliArguments.Options.Count.Should().Be(1);
        cliArguments.Options.Single().IsPresent.Should().BeTrue();
        cliArguments.Option(anOptionShortName).IsPresent.Should().BeTrue();
    }

    [Test]
    public void mark_simple_character_options_repeated_as_present() {
        var anOptionShortName = anOption.ShortName();
        var shortNamePrefix = anOption.ShortNamePrefix();
        var environmentArgs = new[] { $"{shortNamePrefix}{anOptionShortName}{anOptionShortName}" };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName)
            .Build();

        cliArguments.Options.Count.Should().Be(1);
        cliArguments.Options.Single().IsPresent.Should().BeTrue();
    }

    [TestCase('r', "arr")]
    [TestCase('r', "rar")]
    [TestCase('r', "rra")]
    public void do_not_read_simple_character_options_repeated_when_they_are_not_configured(char anOptionShortName, string args) {
        var shortNamePrefix = anOption.ShortNamePrefix();
        var environmentArgs = new[] { $"{shortNamePrefix}{args}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Option(anOptionShortName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- 'a'\r\nTry 'PROGRAM --help' for more information.");
    }

    [TestCase(new[] {'r', 'a' }, "arr")]
    [TestCase(new[] { 'r', 'a' }, "rar")]
    [TestCase(new[] { 'r', 'a' }, "rra")]
    public void mark_multiple_simple_character_options_repeated_as_present(char[] someOptionsShortName, string args) {
        var shortNamePrefix = anOption.ShortNamePrefix();
        var environmentArgs = new[] { $"{shortNamePrefix}{args}" };
        var cliBuilder = CliBuilderFrom(environmentArgs);
        someOptionsShortName.ToList().ForEach(optionShortName => {
                cliBuilder.Option(optionShortName);
        });
        
        var cliArguments = cliBuilder.Build();

        cliArguments.Options.Count.Should().Be(2);
        someOptionsShortName.ToList().ForEach(optionShortName => {
            cliArguments.Option(optionShortName).IsPresent.Should().BeTrue();
        });
    }


    [TestCase("!")]
    [TestCase("Q-")]
    [TestCase("Q-Q")]
    [TestCase("Q-Q")]
    public void trow_exception_when_short_option_is_not_configured(string optionName) {
        var anOptionShortNamePrefix = anOption.ShortNamePrefix();
        var environmentArgs = new[] { $"{anOptionShortNamePrefix}{optionName}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }

}