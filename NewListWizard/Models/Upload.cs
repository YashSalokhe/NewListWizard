namespace NewListWizard.Models
{
    public class Upload
    {
        public Upload()
        {
            MissingField = 0;
            ImportedField = 0;
            csvContents = new List<CsvContent>();

        }

        public IFormFile File { get; set; } = null!;

        public int MissingField { get; set; }
        public int ImportedField { get; set; }
        public IEnumerable<CsvContent> csvContents { get; set; }
    }
}
