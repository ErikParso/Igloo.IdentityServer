using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Quickstart.UI
{
    [Route("api/account/[Controller]")]
    public class RegistrationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RegistrationController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("register")]
        public ViewResult Register([FromQuery]string returnUrl)
            => View("Views/Account/Registration.cshtml", new RegistrationViewModel(returnUrl));

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Views/Account/Registration.cshtml", viewModel);
            }

            var user = userManager.FindByNameAsync(viewModel.Login).Result;
            if (user != null)
            {
                ModelState.AddModelError(nameof(viewModel.Login), "User with same name already exists.");
                return View("Views/Account/Registration.cshtml", viewModel);
            }

            user = new ApplicationUser { UserName = viewModel.Login };

            var result = userManager.CreateAsync(user, viewModel.Password).Result;
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return View("Views/Account/Registration.cshtml", viewModel);
            }

            var namePartitions = viewModel.UserName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            result = userManager.AddClaimsAsync(user, new Claim[]{
                    new Claim(JwtClaimTypes.Name, viewModel.UserName),
                    new Claim(JwtClaimTypes.GivenName, namePartitions[0]),
                    new Claim(JwtClaimTypes.FamilyName, namePartitions[1]),
                    new Claim(JwtClaimTypes.Email, viewModel.Email ?? string.Empty)
                }).Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            return RedirectToAction("login", "account", new { returnUrl = viewModel.ReturnUrl });
        }

        private async Task<bool> ValidatePassword(string password, ApplicationUser user)
        {
            var result = true;
            foreach (var validator in userManager.PasswordValidators)
            {
                var validation = await validator.ValidateAsync(userManager, user, password);
                if (!validation.Succeeded)
                {
                    result = false;
                    foreach (var error in validation.Errors)
                    {
                        ModelState.AddModelError(nameof(RegistrationViewModel.Password), error.Description);
                    }
                }
            }
            return result;
        }

        [HttpPost]
        [Route("cancel")]
        public IActionResult Cancel(RegistrationViewModel viewModel)
            => RedirectToAction("login", "account", new { returnUrl = viewModel.ReturnUrl });
    }
}