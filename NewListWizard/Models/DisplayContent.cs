namespace NewListWizard.Models
{
    public class DisplayContent
    {
        public int MissingField { get; set; }
        public int ImportedField { get; set; }
        public IEnumerable<CsvContent>? csvContents { get; set; }
    }
}
