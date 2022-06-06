using System;
using System.Collections.Generic;

namespace NewListWizard.Models
{
    public partial class WizardList
    {
        public WizardList()
        {
            CsvContents = new HashSet<CsvContent>();
        }

        public int ListId { get; set; }
        public int UserId { get; set; }
        public string ListName { get; set; } = null!;
        public string AssignedTo { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte IsDeleted { get; set; }

        public virtual UserInfo User { get; set; } = null!;
        public virtual ICollection<CsvContent> CsvContents { get; set; }
    }
}
