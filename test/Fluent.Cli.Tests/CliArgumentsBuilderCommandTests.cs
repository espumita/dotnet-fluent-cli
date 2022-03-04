using System;
using Bogus;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliArgumentsBuilderCommandTests {
    private CommandFaker aCommand;

    [SetUp]
    public void SetUp() {
        var faker = new Faker();
        aCommand = new CommandFaker(faker);
    }

    [Test]
    public void get_program_without_command() {
        var environmentArgs = new string[] { };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .Build();

        cliArguments.IsCommandPresent().Should().BeFalse();
    }

    [Test]
    public void throw_argument_exception_when_command_name_is_not_correctly_configured() {
        var environmentArgs = new string[] { };
        var aCommandName = aCommand.NameWithNonAlphanumericalValue();

       Action action = () => CliBuilderFrom(environmentArgs)
            .Command(aCommandName)
            .Build();

       action.Should().Throw<ArgumentException>()
           .And.Message.Should().Be($"'{aCommandName}' is not a valid command, only alpha-numeric chars (a-zA-Z0-1) values can be configured");
    }

    [Test]
    public void get_program_with_a_command() {
        var aCommandName = aCommand.Name();
        var environmentArgs = new [] { aCommandName };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .Command(aCommandName)
            .Build();

        cliArguments.IsCommandPresent().Should().BeTrue();
        var command = cliArguments.Command();
        command.Name.Should().Be(aCommandName);
    }

    [Test]
    public void only_get_first_command_as_command() {
        var aCommandName = aCommand.Name();
        var anotherCommandName = aCommand.Name();
        var environmentArgs = new[] { aCommandName, anotherCommandName };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .Command(aCommandName)
            .Command(anotherCommandName)
            .Build();

        cliArguments.IsCommandPresent().Should().BeTrue();
        var command = cliArguments.Command();
        command.Name.Should().Be(aCommandName);
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }

}