namespace Fluent.Cli.Configuration;

public class ProgramDescriptionsConfiguration {
    public string HeaderDescription { get; set; }
    public string FooterDescription { get; set; }

    public void AddHeaderDescription(string description) {
        HeaderDescription = description;
    }

    public void AddFooterDescription(string description) {
        FooterDescription = description;
    }
}