namespace Fluent.Cli;

public class Option {
    public string ShortName { get; }
    public bool IsPresent { get; }
    
    public Option(string shortName, bool isPresent) {
        ShortName = shortName;
        IsPresent = isPresent;
    }
}