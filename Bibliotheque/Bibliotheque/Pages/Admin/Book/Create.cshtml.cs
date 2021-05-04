using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotheque.Data;
using Bibliotheque.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bibliotheque.Pages.Admin.Book
{
    [Authorize(Roles = Roles.ADMIN_ROLE)]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        [BindProperty]
        public Models.Book Book { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_context.Categories.Any(x => x.Id == Book.CategoryId))
                    {
                        ModelState.AddModelError("Category", "La catégorie n'existe pas !");
                        return Page();
                    }

                    Book.Preface = FileHelper.ConvertToBytes(file);
                    _context.Books.Add(Book);
                    if (_context.SaveChanges() > 0)
                    {
                        return RedirectToPage("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Save", "La sauvegarde a échouée !");
                    }

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Exception", $"Une erreur est survenue : {e.Message}");
                }
            }
            return Page();
        }
    }
}
