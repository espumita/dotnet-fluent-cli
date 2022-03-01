using Bogus;

namespace Fluent.Cli.Tests.Utils; 

public class OptionFaker {
    private readonly Faker faker;
    private const char _0 = '\u0030';
    private const char _9 = '\u0039';
    private const char A = '\u0041';
    private const char Z = '\u005A';
    private const char a = '\u0061';
    private const char z = '\u007A';
    private const string AlphanumericChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private static readonly string[] ShortOptionsPrefixes = { "-" };
    private static readonly string[] LongOptionsPrefixes = { "--" };
    private static readonly char[] StringEscapeSequences = { '\a', '\b', '\f', '\n', '\r', '\t', '\v' };
    private const char Space = '\u0020';

    public OptionFaker(Faker faker) {
        this.faker = faker;
    }

    public char ShortName() {
        var f = faker.Random.Byte(0, 2);
        return f switch {
            0 => faker.Random.Char(_0, _9),
            1 => faker.Random.Char(A, Z),
            _ => faker.Random.Char(a, z),
        };
    }

    public char ShortNameWithNonAlphanumericalValue() {
        var f = faker.Random.Byte(0, 3);
        var shortName = f switch {
            0 => faker.Random.Char('\u0000', '\u002f'),
            1 => faker.Random.Char('\u003A', '\u0040'),
            2 => faker.Random.Char('\u005B', '\u0060'),
            _ => faker.Random.Char('\u007B', '\uffff'),
        };
        return shortName == Space || StringEscapeSequences.ToString()!.Contains(shortName)
            ? ShortNameWithNonAlphanumericalValue()
            : shortName;
    }

    public object ShortNamePrefix() {
        var f = faker.Random.Int(0, ShortOptionsPrefixes.Length - 1);
        return ShortOptionsPrefixes[f];
    }

    public string LongName() {
        return faker.Random.String2(1, 100, AlphanumericChars);
    }

    public string LongNameWithNonAlphanumericalValue() {
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

    public object LongNamePrefix() {
        var f = faker.Random.Int(0, LongOptionsPrefixes.Length - 1);
        return LongOptionsPrefixes[f];
    }

    public string ArgumentName() {
        return faker.Random.String2(1, 100, AlphanumericChars);
    }

    public string ArgumentValue() {
        var f = faker.Random.Byte(0, 1);
        var argumentValue = faker.Random.String(1, 100, '\u0000', '\uffff');
        return RemoveSpace(RemoveStringEscapeSequences(argumentValue));
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