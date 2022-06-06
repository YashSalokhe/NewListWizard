using System;
using System.Collections.Generic;

namespace NewListWizard.Models
{
    public partial class CsvContent
    {
        public int CsvId { get; set; }
        public int ListId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual WizardList List { get; set; } = null!;
    }
}
