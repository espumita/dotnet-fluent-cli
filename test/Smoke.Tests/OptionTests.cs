using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Smoke.Tests {
    public class OptionTests {
        private PromtForTests promt;
        private const string executable = "Smoke.Tests.Promt.exe";

        [SetUp]
        public void Setup() {
            promt = new PromtForTests(executable);
        }

        [Test]
        public async Task option_not_enabled() {
            var environmentArgs = string.Empty;

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().Be("Verbose option is not present");
        }

        [Test]
        public async Task option_enabled() {
            var environmentArgs = "--verbose";

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().Be("Verbose option is present");
        }

    }
}