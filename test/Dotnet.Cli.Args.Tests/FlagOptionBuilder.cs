using Bogus;

namespace Dotnet.Cli.Args.Tests;

public class FlagOptionBuilder {

    public FlagOption BuildWithTestValues() {
        return new Faker<FlagOption>()
            .RuleFor(flagOption => flagOption.ShortName, x => x.Random.String2(1, 252611))
            .Generate();
    }
}