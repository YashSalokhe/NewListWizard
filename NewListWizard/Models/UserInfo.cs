using System;
using System.Collections.Generic;

namespace NewListWizard.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            WizardLists = new HashSet<WizardList>();
        }

        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public DateTime? LastLoggedIn { get; set; }
        public int FailedAttempts { get; set; }
        public byte IsRememberMe { get; set; }
        public byte IsLockedOut { get; set; }

        public virtual ICollection<WizardList> WizardLists { get; set; }
    }
}
