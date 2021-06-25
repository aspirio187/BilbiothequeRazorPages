using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Bibliotheque.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Bibliotheque.Data;

namespace Bibliotheque.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager ??
                throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ??
                throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ??
                throw new ArgumentNullException(nameof(roleManager));
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "L'adresse email est requise !")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Le mot de passe est requis !")]
            [StringLength(100, ErrorMessage = "Le {0} doit faire au moins {2} et au plus {1} caractères de long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmation du mot de passe")]
            [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation de mot de passe sont différents !")]
            public string ConfirmPassword { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Le prénom est requis !")]
            [Display(Name = "Prénom")]
            public string FirstName { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Le nom de famille est requis !")]
            [Display(Name = "Nom")]
            public string LastName { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Le numéro de téléphone est requis !")]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Numéro")]
            public string PhoneNumber { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync(Roles.ADMIN_ROLE))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.ADMIN_ROLE));
            }
            if (!await _roleManager.RoleExistsAsync(Roles.CLIENT_ROLE))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.CLIENT_ROLE));
            }

            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Utilisateur crée avec succès.");

                    /*************************************************************/
                    /* Pour créer un compte Administrateur, Il faut décommenter  */
                    /* ligne 122 et commenter la ligne 123. Une fois que le      */
                    /* compte admin est crée, et qu'on ne veut maintenant plus   */
                    /* que des clients, on refait l'inverse : commenter la ligne */
                    /* 122 et décommenter la ligne 123                           */
                    /*************************************************************/

                    bool adminUserExist = _context.UserRoles.Any(ur => ur.RoleId == _context.Roles.FirstOrDefault(r => r.Name.Equals(Roles.ADMIN_ROLE)).Id);
                    IdentityResult roleResult;

                    if (!adminUserExist)
                    {
                        roleResult = await _userManager.AddToRoleAsync(user, Roles.ADMIN_ROLE);
                        _logger.LogInformation($"Utilisateur ajouté au role : {Roles.ADMIN_ROLE}");
                    }
                    else
                    {
                        roleResult = await _userManager.AddToRoleAsync(user, Roles.CLIENT_ROLE);
                        _logger.LogInformation($"Utilisateur ajouté au role : {Roles.CLIENT_ROLE}");
                    }

                    if (roleResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("/");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
