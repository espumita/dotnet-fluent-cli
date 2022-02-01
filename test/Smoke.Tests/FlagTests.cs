using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Smoke.Tests {
    public class FlagTests {
        private ConsoleAppForTests consoleApp;
        private const string executable = "Smoke.Tests.ConsoleApp.exe";

        [SetUp]
        public void Setup() {
            consoleApp = new ConsoleAppForTests(executable);
        }

        [Test]
        public async Task flag_not_enabled() {
            var args = string.Empty;

            var consoleOutput = await consoleApp.RunWithArgs(args);

            consoleOutput.Should().Be("Option flag is not present");
        }

        [Test]
        public async Task flag_enabled() {
            var args = "--option";

            var consoleOutput = await consoleApp.RunWithArgs(args);

            consoleOutput.Should().Be("Option flag is present");
        }

    }
}