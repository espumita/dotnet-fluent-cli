using System;
using Bogus;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests;

public class CliArgumentsBuilderLongOptionArgumentsTests
{

    private Faker faker;
    private OptionFaker anOption;

    [SetUp]
    public void SetUp() {
        faker = new Faker();
        anOption = new OptionFaker(faker);
    }

    [Test]
    public void throw_argument_exception_when_a_null_is_received() {
        var anOptionLongName = anOption.LongName();
        var environmentArguments = new string[] { };
        Action action = () => {
            CliBuilderFrom(environmentArguments)
                .LongOption(anOptionLongName)
                    .WithArgument(null)
                .Build();
        };

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be("Argument name cannot be null or empty");
    }

    [Test]
    public void throw_exception_when_argument_is_not_configured_yet() {
        var anOptionLongName = anOption.LongName();
        var environmentArguments = new string[] { };
        var cliArguments = CliBuilderFrom(environmentArguments)
            .LongOption(anOptionLongName)
            .Build();
        var option = cliArguments.Option(anOptionLongName);

        Action action = () => {
            option.Argument();
        };

        action.Should().Throw<ArgumentIsNotConfiguredException>()
            .And.Message.Should().Be($"Argument for option '{anOptionLongName}' has not been configured yet, add it to the builder first.");
    }

    [Test]
    public void throw_exception_when_option_is_not_configured_yet_and_try_to_configure_an_argument() {
        var argumentName = anOption.ArgumentName();
        var environmentArguments = new string[] { };

        Action action = () => {
            CliBuilderFrom(environmentArguments)
                .WithArgument(argumentName)
                .Build();
        };

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"Argument '{argumentName}' could not be configured, you need to configure an Option first.");
    }

    [Test]
    public void get_a_long_option_argument_value_when_argument_is_after_equals_sign() {
        var anOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var environmentArguments = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}={argumentValue}" };
        var cliArguments = CliBuilderFrom(environmentArguments)
            .LongOption(anOptionLongName)
                .WithArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionLongName);

        var argument = option.Argument();

        argument.Name.Should().Be(argumentName);
        argument.Value.Should().Be(argumentValue);
    }

    [Test]
    public void do_not_get_a_long_option_argument_value_when_argument_is_not_after_equals_sign() {
        var anOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentName = anOption.ArgumentName();
        var environmentArguments = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}=" };
        var cliArguments = CliBuilderFrom(environmentArguments)
            .LongOption(anOptionLongName)
            .WithArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionLongName);

        var argument = option.Argument();

        option.IsPresent.Should().BeTrue();
        argument.Name.Should().Be(argumentName);
        argument.Value.Should().Be(string.Empty);
    }

    [Test]
    public void do_not_get_a_long_option_argument_value_when_equals_sign_is_not_present() {
        var anOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var environmentArguments = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}{argumentValue}" };
        var cliArguments = CliBuilderFrom(environmentArguments)
            .LongOption(anOptionLongName)
            .WithArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionLongName);

        var argument = option.Argument();

        option.IsPresent.Should().BeFalse();
        argument.Name.Should().Be(argumentName);
        argument.Value.Should().BeNull();
    }

    [Test]
    public void trow_exception_when_long_option_with_argument_is_not_configured() {
        var anOptionLongName = anOption.LongName();
        var anotherOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var environmentArguments = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}{anotherOptionLongName}={argumentValue}" };

        Action action = () => CliBuilderFrom(environmentArguments)
            .LongOption(anOptionLongName)
            .WithArgument(argumentName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{anOptionLongName}{anotherOptionLongName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args)
    {
        return CliArgumentsBuilder.With(args);
    }

}