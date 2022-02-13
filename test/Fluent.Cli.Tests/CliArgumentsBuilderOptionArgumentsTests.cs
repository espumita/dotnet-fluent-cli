using System;
using Bogus;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliArgumentsBuilderOptionArgumentsTests {

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
            .And.Message.Should().Be("argument name cannot be null or empty");
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }

}