using System.ComponentModel.DataAnnotations;

namespace BudgetApp.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email nie może pozostać pusty.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Hasło nie może pozostać puste.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
    }
}