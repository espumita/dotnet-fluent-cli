using System;
using Bogus;
using Fluent.Cli.Tests.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Fluent.Cli.Tests; 

public class CliArgumentsBuilderArgumentsTests {
    private ArgumentFaker anArgument;

    [SetUp]
    public void Setup() {
        var faker = new Faker();
        anArgument = new ArgumentFaker(faker);
    }

    [Test]
    public void get_program_without_arguments() {
        var environmentArgs = new string[] { };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .Build();

        cliArguments.Arguments.Should().BeEmpty();
    }

    [Test]
    public void get_program_first_argument() {
        var anArgumentValue = anArgument.ArgumentValue();
        var environmentArgs = new[] { $"{anArgumentValue}" };

        var cliArguments = CliBuilderFrom(environmentArgs)
            .Build();

        var argument = cliArguments.Argument("$0");
        argument.Name.Should().Be("$0");
        argument.Value.Should().Be(anArgumentValue);
    }

    [TestCase(new[] { "$0" }, ".")]
    [TestCase(new[] { "$0", "$1" }, ".", "/")]
    [TestCase(new[] { "$0", "$1", "$2" }, ".", "/", ".")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4" }, ".", "/", ".", "/", ".")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4", "$5" }, ".", "/", ".", "/", ".", "/")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6" }, ".", "/", ".", "/", ".", "/", ".")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7" }, ".", "/", ".", "/", ".", "/", ".", "/")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7", "$8" }, ".", "/", ".", "/", ".", "/", ".", "/", ".")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7", "$8", "$9" }, ".", "/", ".", "/", ".", "/", ".", "/", ".", "/")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7", "$8", "$9", "$10" }, ".", "/", ".", "/", ".", "/", ".", "/", ".", "/", ".")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7", "$8", "$9", "$10", "$11" }, ".", "/", ".", "/", ".", "/", ".", "/", ".", "/", ".", "/")]
    [TestCase(new[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7", "$8", "$9", "$10", "$11", "$12" }, ".", "/", ".", "/", ".", "/", ".", "/", ".", "/", ".", "/", ".")]
    public void get_program_with_multiple_arguments(string[] argumentsNames, params string[] environmentArgs) {
        
        var cliArguments = CliBuilderFrom(environmentArgs)
            .Build();

        for (int index = 0; index < argumentsNames.Length; index++) {
            var argument = cliArguments.Argument(argumentsNames[index]);
            argument.Name.Should().Be(argumentsNames[index]);
            argument.Value.Should().Be(environmentArgs[index]);
        }
    }


    [TestCase(new[] { "$0" }, new[] { "file1" }, "-r", "file1")]
    [TestCase(new[] { "$0" }, new[] { "file1" }, "file1", "-r")]
    [TestCase(new[] { "$0", "$1" }, new[] { "file1", "file2" }, "file1", "-r", "file2", "-s")]
    [TestCase(new[] { "$0", "$1" }, new[] { "file1", "file2" }, "file1", "file2", "-r", "-s")]
    [TestCase(new[] { "$0", "$1" }, new[] { "file1", "file2" }, "-r", "-s", "file1", "file2")]
    [TestCase(new[] { "$0", "$1" }, new[] { "file1", "file2" },  "file1", "-r", "file2")]
    [TestCase(new[] { "$0", "$1" }, new[] { "file1", "file2" },  "file1", "-rs", "file2")]
    [TestCase(new[] { "$0", "$1" }, new[] { "file1", "file2" },  "-rsrsrsr", "file1", "-rsrsrsr", "file2", "-rsrsrsr")]
    public void get_program_with_argument_and_option_configured(string[] argumentsNames, string[] argumentsValues, params string[] environmentArgs) {
        
        var cliArguments = CliBuilderFrom(environmentArgs)
            .Option('r')
            .Option('s')
            .Build();

        for (int index = 0; index < argumentsNames.Length; index++) {
            var argument = cliArguments.Argument(argumentsNames[index]);
            argument.Name.Should().Be(argumentsNames[index]);
            argument.Value.Should().Be(argumentsValues[index]);
        }
    }

    [TestCase("-r", "file1")]
    [TestCase("file1", "-r")]
    [TestCase("file1", "-r", "file2", "-s")]
    [TestCase("file1", "file2", "-r", "-s")]
    [TestCase("-r", "-s", "file1", "file2")]
    [TestCase("file1", "-r", "file2")]
    [TestCase("file1", "-rs", "file2")]
    [TestCase("-rsrsrsr", "file1", "-rsrsrsr", "file2", "-rsrsrsr")]
    public void get_argument_exception_when_options_are_not_configured(params string[] environmentArgs) {

        Action action = () => CliBuilderFrom(environmentArgs)
            .Build();

        action.Should().Throw<ArgumentException>()
            .And.Message.Should().Be($"PROGRAM: invalid option -- 'r'\r\nTry 'PROGRAM --help' for more information.");
    }

    private static CliArgumentsBuilder CliBuilderFrom(string[] args) {
        return CliArgumentsBuilder.With(args);
    }
}
