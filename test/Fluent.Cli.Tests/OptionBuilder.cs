using Bogus;

namespace Fluent.Cli.Tests;

public class OptionBuilder {

    public Option BuildWithTestValues() {
        return new Faker<Option>()
            .RuleFor(option => option.ShortName, f => f.Random.String2(1, 252611))
            .Generate();
    }
}