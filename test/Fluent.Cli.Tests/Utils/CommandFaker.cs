using Bogus;

namespace Fluent.Cli.Tests.Utils; 

public class CommandFaker {
    private readonly Faker faker;
    private const string AlphanumericChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static readonly char[] StringEscapeSequences = { '\a', '\b', '\f', '\n', '\r', '\t', '\v' };
    private const char Space = '\u0020';

    public CommandFaker(Faker faker) {
        this.faker = faker;
    }

    public string Name() {
        return faker.Random.String2(1, 100, AlphanumericChars);
    }

    public string NameWithNonAlphanumericalValue() {
        var f = faker.Random.Byte(0, 4);
        var longName = f switch {
            0 => faker.Random.String(1, 100, '\u0000', '\u002c'),
            1 => faker.Random.String(1, 100, '\u002e', '\u002f'),
            2 => faker.Random.String(1, 100, '\u003A', '\u0040'),
            3 => faker.Random.String(1, 100, '\u005B', '\u0060'),
            _ => faker.Random.String(1, 100, '\u007B', '\uffff'),
        };
        return RemoveSpace(RemoveStringEscapeSequences(longName));
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
}