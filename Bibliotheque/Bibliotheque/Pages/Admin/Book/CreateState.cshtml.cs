using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotheque.Data;
using Bibliotheque.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bibliotheque.Pages.Admin.Book
{
    [Authorize(Roles = Roles.ADMIN_ROLE)]
    public class CreateStateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateStateModel(ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public List<SelectListItem> States { get; set; } = new();

        [BindProperty]
        public BookCopy BookCopy { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            List<string> availableStates = BookStates.GetBookStates().Where(s => !_context.BookCopies.Any(x => x.BookId == id && x.State == s)).ToList();

            foreach (var bookState in availableStates)
            {
                States.Add(new SelectListItem { Value = bookState, Text = bookState });
            }
            BookCopy = new BookCopy() { BookId = id.Value };
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!BookStates.StateExist(BookCopy.State))
            {
                return BadRequest();
            }

            if (!_context.Books.Any(x => x.Id == BookCopy.BookId))
            {
                return BadRequest();
            }

            try
            {
                _context.BookCopies.Add(BookCopy);
                if (_context.SaveChanges() > 0)
                {
                    return RedirectToPage("Edit", new { id = BookCopy.BookId });
                }
                return Page();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
