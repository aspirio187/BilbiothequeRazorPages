using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bibliotheque.Data;
using Bibliotheque.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bibliotheque.Pages.Admin.Book
{
    [Authorize(Roles = Roles.ADMIN_ROLE)]
    public class UpdateBookCopyModel : PageModel
    {
        private readonly Bibliotheque.Data.ApplicationDbContext _context;

        public UpdateBookCopyModel(Bibliotheque.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> AvailableStates { get; set; } = new List<SelectListItem>();

        [BindProperty]
        public BookCopy BookCopy { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            BookCopy = await _context.BookCopies.FirstOrDefaultAsync(m => m.Id == id);

            if (BookCopy is null)
            {
                return NotFound();
            }

            var tempStates = BookStates.GetBookStates().Where(s => !_context.BookCopies.Any(x => x.BookId == BookCopy.BookId && x.State == s) || s == BookCopy.State).ToList();

            foreach (var item in tempStates)
            {
                AvailableStates.Add(new SelectListItem { Value = item, Text = item });
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!_context.Books.Any(x => x.Id == BookCopy.BookId))
            {
                return Page();
            }

            var copy = _context.BookCopies.FirstOrDefault(x => x.Id == BookCopy.Id);

            if (copy is null)
            {
                return NotFound();
            }

            if (!BookStates.StateExist(BookCopy.State))
            {
                return Page();
            }

            copy.State = BookCopy.State;
            copy.Quantity = BookCopy.Quantity;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("Edit", new { id = BookCopy.BookId });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
