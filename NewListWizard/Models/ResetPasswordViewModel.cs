namespace NewListWizard.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password Should Match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
