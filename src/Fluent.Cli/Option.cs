namespace Fluent.Cli;

public class Option {
    public char? ShortName { get; }
    public string Name { get; }
    public bool IsPresent { get; }
    
    public Option(char? shortName, string name, bool isPresent) {
        ShortName = shortName;
        Name = name;
        IsPresent = isPresent;
    }

}