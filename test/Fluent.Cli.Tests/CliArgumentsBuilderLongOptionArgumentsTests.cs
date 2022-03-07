using System;
using Bogus;
using Fluent.Cli.Exceptions;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests;

public class CliArgumentsBuilderLongOptionArgumentsTests {
    private OptionFaker anOption;

    [SetUp]
    public void SetUp() {
        var faker = new Faker();
        anOption = new OptionFaker(faker);
    }

    [Test]
    public void throw_argument_exception_when_a_null_is_received() {
        var anOptionLongName = anOption.LongName();
        var environmentArgs = new string[] { };
        Action action = () => {
            CliBuilderFrom(environmentArgs)
                .LongOption(anOptionLongName)
                    .WithOptionArgument(null)
                .Build();
        };

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be("Argument name cannot be null or empty");
    }

    [Test]
    public void throw_exception_when_argument_is_not_configured_yet() {
        var anOptionLongName = anOption.LongName();
        var environmentArgs = new string[] { };
        var cliArguments = CliBuilderFrom(environmentArgs)
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
    public void get_a_long_option_argument_value_when_argument_is_after_equals_sign() {
        var anOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}={argumentValue}" };
        var cliArguments = CliBuilderFrom(environmentArgs)
            .LongOption(anOptionLongName)
                .WithOptionArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionLongName);

        var argument = option.Argument();

        argument.Name.Should().Be(argumentName);
        argument.Value.Should().Be(argumentValue);
    }


    [Test]
    public void get_multiple_long_option_argument_value_when_argument_is_after_equals_sign() {
        var anOptionLongName = anOption.LongName();
        var antherOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentName = anOption.ArgumentName();
        var anotherArgumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var anotherArgumentValue = anOption.ArgumentValue();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}={argumentValue}", $"{anOptionLongNamePrefix}{antherOptionLongName}={anotherArgumentValue}" };
        
        var cliArguments = CliBuilderFrom(environmentArgs)
            .LongOption(anOptionLongName)
                .WithOptionArgument(argumentName)
            .LongOption(antherOptionLongName)
                .WithOptionArgument(anotherArgumentName)
            .Build();
        
        var option = cliArguments.Option(anOptionLongName);
        var argument = option.Argument();
        argument.Name.Should().Be(argumentName);
        argument.Value.Should().Be(argumentValue);
        var anotherOption = cliArguments.Option(antherOptionLongName);
        var anotherArgument = anotherOption.Argument();
        anotherArgument.Name.Should().Be(anotherArgumentName);
        anotherArgument.Value.Should().Be(anotherArgumentValue);
    }

    [Test]
    public void do_not_get_a_long_option_argument_value_when_argument_is_not_after_equals_sign() {
        var anOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentName = anOption.ArgumentName();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}=" };
        var cliArguments = CliBuilderFrom(environmentArgs)
            .LongOption(anOptionLongName)
                .WithOptionArgument(argumentName)
            .Build();
        var option = cliArguments.Option(anOptionLongName);

        var argument = option.Argument();

        argument.Name.Should().Be(argumentName);
        argument.Value.Should().Be(string.Empty);
    }

    [Test]
    public void do_not_get_a_long_option_argument_value_when_equals_sign_is_not_present() {
        var anOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentValue = anOption.ArgumentValue();
        var argumentName = anOption.ArgumentName();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}{argumentValue}" };
        
        Action action = () => CliBuilderFrom(environmentArgs)
            .LongOption(anOptionLongName)
                .WithOptionArgument(argumentName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{anOptionLongName}{argumentValue}'\r\nTry 'PROGRAM --help' for more information.");
    }

    [Test]
    public void trow_exception_when_long_option_with_argument_is_not_configured() {
        var anOptionLongName = anOption.LongName();
        var anotherOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentName = anOption.ArgumentName();
        var argumentValue = anOption.ArgumentValue();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}{anotherOptionLongName}={argumentValue}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .LongOption(anOptionLongName)
                .WithOptionArgument(argumentName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- '{anOptionLongName}{anotherOptionLongName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    [Test]
    public void trow_exception_when_long_option_is_configured_but_argument_is_not_configured() {
        var anOptionLongName = anOption.LongName();
        var anOptionLongNamePrefix = anOption.LongNamePrefix();
        var argumentValue = anOption.ArgumentValue();
        var environmentArgs = new[] { $"{anOptionLongNamePrefix}{anOptionLongName}={argumentValue}" };

        Action action = () => CliBuilderFrom(environmentArgs)
            .LongOption(anOptionLongName)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: option -- '{anOptionLongName}' cannot be used with arguments.\r\nTry 'PROGRAM --help' for more information.");
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }

}