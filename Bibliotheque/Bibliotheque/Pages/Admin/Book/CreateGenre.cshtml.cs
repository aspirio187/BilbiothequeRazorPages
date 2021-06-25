using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bibliotheque.Data;
using Bibliotheque.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Bibliotheque.Helpers;

namespace Bibliotheque.Pages.Admin.Book
{
    [Authorize(Roles = Roles.ADMIN_ROLE)]
    public class CreateGenreModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateGenreModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string ReturnUrl { get; set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            return Page();
        }

        [BindProperty]
        public Genre Genre { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Genre.Name = Genre.Name.FirstCharToUpper();

            if(await _context.Genres.AnyAsync(g => g.Name.Equals(Genre.Name)))
            {
                ModelState.AddModelError(string.Empty, "Le genre existe déjà !");
                return Page();
            }

            _context.Genres.Add(Genre);
            
            if(await _context.SaveChangesAsync() < 1)
            {
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'enregistrement !");
            }

            if(string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToPage("./Index");
            }
            return LocalRedirect(returnUrl);
        }
    }
}
