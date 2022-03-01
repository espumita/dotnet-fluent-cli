using Bogus;

namespace Fluent.Cli.Tests.Utils;

public class ArgumentFaker {
    private readonly Faker faker;
    private static readonly char[] StringEscapeSequences = { '\a', '\b', '\f', '\n', '\r', '\t', '\v' };
    private const char Space = '\u0020';
    private const char HyphenMinus = '\u002D';


    public ArgumentFaker(Faker faker) {
        this.faker = faker;
    }

    public string ArgumentValue() {
        var f = faker.Random.Byte(0, 1);
        var argumentValue = faker.Random.String(1, 100, '\u0000', '\uffff');
        return RemoveStartedHyphenMinus(RemoveSpace(RemoveStringEscapeSequences(argumentValue)));
    }

    private static string RemoveStringEscapeSequences(string value) {
        foreach (var escapeSequence in StringEscapeSequences) {
            value = value.Replace(escapeSequence, '\0');
        }
        return value;
    }

    private string RemoveSpace(string value) {
        return value.Replace(Space, '\0');
    }

    private string RemoveStartedHyphenMinus(string value) {
        return value[0].Equals(HyphenMinus)
            ? RemoveStartedHyphenMinus(value.Substring(1))
            : value;
    }
}