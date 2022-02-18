﻿using System;
using Bogus;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliArgumentsBuilderShortOptionArgumentsTests {

    private Faker faker;
    private OptionFaker anOption;

    [SetUp]
    public void SetUp() {
        faker = new Faker();
        anOption = new OptionFaker(faker);
    }

    [Test]
    public void throw_argument_exception_when_a_null_is_received() {
        var anOptionShortName = anOption.ShortName();
        var environmentArguments = new string[]{};
        Action action = () => {
            CliBuilderFrom(environmentArguments)
                .Option(anOptionShortName)
                    .WithArgument(null)
                .Build();
        };

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be("Argument name cannot be null or empty");
    }

    [Test]
    public void throw_exception_when_argument_is_not_configured_yet() {
        var anOptionShortName = anOption.ShortName();
        var environmentArguments = new string[] { };
        var cliArguments = CliBuilderFrom(environmentArguments)
            .Option(anOptionShortName)
            .Build();
        var option = cliArguments.Option(anOptionShortName);

        Action action = () => {
            option.Argument();
        };

        action.Should().Throw<ArgumentIsNotConfiguredException>()
            .And.Message.Should().Be($"Argument for option '{anOptionShortName}' has not been configured yet, add it to the builder first.");
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
    public void get_a_short_option_argument_value_when_argument_is_after_equals_sign() {
        var anOptionShortName = anOption.ShortName();
        var anOptionShortNamePrefix = anOption.ShortNamePrefix();
        var argumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var environmentArguments = new [] { $"{anOptionShortNamePrefix}{anOptionShortName}={argumentValue}"};
        var cliArguments = CliBuilderFrom(environmentArguments)
            .Option(anOptionShortName)
                .WithArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionShortName);

        var argument = option.Argument();

        argument.Name.Should().Be(argumentName);
        argument.Value.Should().Be(argumentValue);
    }

    [Test]
    public void do_not_get_a_short_option_argument_value_when_argument_is_not_after_equals_sign() {
        var anOptionShortName = anOption.ShortName();
        var anOptionShortNamePrefix = anOption.ShortNamePrefix();
        var argumentName = anOption.ArgumentName();
        var environmentArguments = new[] { $"{anOptionShortNamePrefix}{anOptionShortName}=" };
        var cliArguments = CliBuilderFrom(environmentArguments)
            .Option(anOptionShortName)
            .WithArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionShortName);

        var argument = option.Argument();

        option.IsPresent.Should().BeTrue();
        argument.Name.Should().Be(argumentName);
        argument.Value.Should().Be(string.Empty);
    }

    [Test]
    public void do_not_get_a_short_option_argument_value_when_equals_sign_is_not_present() {
        var anOptionShortName = anOption.ShortName();
        var anOptionShortNamePrefix = anOption.ShortNamePrefix();
        var argumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var environmentArguments = new[] { $"{anOptionShortNamePrefix}{anOptionShortName}{argumentValue}" };
        var cliArguments = CliBuilderFrom(environmentArguments)
            .Option(anOptionShortName)
            .WithArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionShortName);

        var argument = option.Argument();

        option.IsPresent.Should().BeFalse();
        argument.Name.Should().Be(argumentName);
        argument.Value.Should().BeNull();
    }

    [Test]
    public void a() { //TODO throw exeption before long is implemented
        var anOptionShortName = anOption.ShortName();
        var anotherOptionShortName = anOption.ShortName();
        var anOptionShortNamePrefix = anOption.ShortNamePrefix();
        var argumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var environmentArguments = new[] { $"{anOptionShortNamePrefix}{anOptionShortName}{anotherOptionShortName}={argumentValue}" };
        var cliArguments = CliBuilderFrom(environmentArguments)
            .Option(anOptionShortName)
            .WithArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionShortName);

        var argument = option.Argument();

        option.IsPresent.Should().BeFalse();
        argument.Name.Should().Be(argumentName);
        argument.Value.Should().BeNull();
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }

}