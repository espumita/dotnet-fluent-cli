using System;
using Bogus;
using Fluent.Cli.Exceptions;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests;
public class CliArgumentsBuilderConfigurationTests {
    private Faker faker;
    private OptionFaker anOption;

    [SetUp]
    public void SetUp() {
        faker = new Faker();
        anOption = new OptionFaker(faker);
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

    [Test]
    public void throw_option_is_not_configured_exception_when_try_to_get_option_by_short_name_not_but_is_not_configured() {
        var anOptionShortName = anOption.ShortName();
        var environmentArgs = new string[] { };
        var cli = CliBuilderFrom(environmentArgs)
            .Build();

        Action action = () => cli.Option(anOptionShortName);

        action.Should().Throw<OptionIsNotConfiguredException>()
            .And.Message.Should().Be($"Option -- '{anOptionShortName}' has not been configured yet, add it to the builder first.");
    }

    [Test]
    public void throw_option_is_not_configured_exception_when_try_to_get_option_by_long_name_not_configured() {
        var anOptionLongName = anOption.LongName();
        var environmentArgs = new string[] { };
        var cli = CliBuilderFrom(environmentArgs)
            .Build();

        Action action = () => cli.Option($"{anOptionLongName}");

        action.Should().Throw<OptionIsNotConfiguredException>()
            .And.Message.Should().Be($"Option -- '{anOptionLongName}' has not been configured yet, add it to the builder first.");
    }

    [Test]
    public void throw_argument_exception_when_read_an_option_not_configured_by_sort_name() {
        var anOptionShortName = anOption.ShortName();
        var anOptionShortNamePrefix = anOption.ShortNamePrefix();
        var environmentArgs = new[] { $"{anOptionShortNamePrefix}{anOptionShortName}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{anOptionShortName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    [Test]
    public void throw_argument_exception_when_read_an_option_not_configured_by_long_name() {
        var anOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{anOptionLongName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    [TestCase("a-a", "-a-a")]
    [TestCase("a-a", "a-a-")]
    [TestCase("a-a", "--a-a")]
    [TestCase("a-a", "-a-a--")]
    public void throw_option_is_not_configured_exception_when_try_to_get_option_by_long_name_with_hyphen_minus_characters_not_configured(string anOptionLongName, string args) {
        var longNamePrefix = anOption.LongNamePrefix();
        var environmentArgs = new [] { $"{longNamePrefix}{args}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{args}'\r\nTry 'PROGRAM --help' for more information.");
    }


    [Test]
    public void throw_exception_when_trying_to_configure_multiple_arguments_for_the_same_option() {
        var anOptionShortName = anOption.ShortName();
        var anOptionArgumentName = anOption.ArgumentName();
        var environmentArgs = new string[] { };

        Action action = () =>
            CliBuilderFrom(environmentArgs)
                .Option(anOptionShortName)
                    .WithArgument(anOptionArgumentName)
                    .WithArgument(anOptionArgumentName)
                .Build();

        action.Should().Throw<OptionWithMultipleArgumentsAreNotSupportedException>()
            .And.Message.Should().Be($"Option -- '{anOptionShortName}' can only be configured with a single argument. If you need multiple arguments, consider use a command instead.");
    }
    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }

}