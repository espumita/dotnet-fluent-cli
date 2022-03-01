using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Smoke.Tests {
    public class CliTests {
        private PromtForTests promt;
        private const string executable = "Smoke.Tests.Promt.exe";

        [SetUp]
        public void Setup() {
            promt = new PromtForTests(executable);
        }

        [Test]
        public async Task show_program_name() {
            var environmentArgs = "--show-program-name";

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().EndWith("Smoke.Tests.Promt");
        }

        [Test]
        public async Task show_program_version() {
            var environmentArgs = "--show-program-version";

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().EndWith("1.0.0");
        }

        [Test]
        public async Task do_not_show_program_as_first_argument() {
            var environmentArgs = "--show-arguments";

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().BeEmpty();
        }

        [Test]
        public async Task do_two_arguments() {
            var environmentArgs = "--show-arguments file1 file2";

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().Be("$0:file1:$1:file2:");
        }

        [Test]
        public async Task option_not_enabled() {
            var environmentArgs = string.Empty;

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().Be(string.Empty);
        }

        [Test]
        public async Task option_enabled() {
            var environmentArgs = "--verbose";

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().Be("Verbose option is present");
        }

        [Test]
        public async Task show_version_when_short_option_is_present() {
            var environmentArgs = "-v";

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().Be("Smoke.Tests.Promt version 1.0.0");
        }

        [Test]
        public async Task show_version_when_long_option_is_present() {
            var environmentArgs = "--version";

            var consoleOutput = await promt.RunWithArguments(environmentArgs);

            consoleOutput.Should().Be("Smoke.Tests.Promt version 1.0.0");
        }

    }
}