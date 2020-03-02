using System.ComponentModel.DataAnnotations;

namespace BudgetApp.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Adres email nie może pozostać pusty.")]
        [EmailAddress(ErrorMessage = "Adres email nie jest poprawny.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło nie może pozostać puste.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć conajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdzenie hasła")]
        [Compare("Password", ErrorMessage = "Hasła nie pasują do siebie.")]
        public string ConfirmPassword { get; set; }
    }
}