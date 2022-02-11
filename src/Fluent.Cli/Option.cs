namespace Fluent.Cli;

public class Option {
    public string ShortName { get; }
    public string Name { get; }
    public bool IsPresent { get; }
    
    public Option(string shortName, string name, bool isPresent) {
        ShortName = shortName;
        Name = name;
        IsPresent = isPresent;
    }
}