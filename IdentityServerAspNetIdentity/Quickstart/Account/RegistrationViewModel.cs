using IdentityServerAspNetIdentity.Validation;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Quickstart.UI
{
    public class RegistrationViewModel
    {
        public RegistrationViewModel(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public RegistrationViewModel()
        {

        }

        [Display(Name = "Login")]
        [Required, MaxLength(256)]
        public string Login { get; set; }

        [Display(Name = "Password")]
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Name")]
        [Required, MaxLength(256), NameSurnameValidation]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required, EmailAddress]
        public string Email { get; set; }

        public string ReturnUrl { get; set; }
    }
}
