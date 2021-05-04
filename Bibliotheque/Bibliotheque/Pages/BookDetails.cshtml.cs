using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bibliotheque.Data;
using Bibliotheque.Models;

namespace Bibliotheque.Pages
{
    public class BookDetailsModel : PageModel
    {
        private readonly Bibliotheque.Data.ApplicationDbContext _context;

        public BookDetailsModel(Bibliotheque.Data.ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public Book Book { get; set; }
        public List<BookCopy> BookCopies { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            // Query possible mais il m'a pris 9 secondes

            //Book = (from book in _context.Books
            //        from genre in _context.Genres
            //        join category in _context.Categories on book.CategoryId equals category.Id
            //        join state in _context.BookCopies on book.Id equals state.BookId
            //        where book.Genres.Contains(genre)
            //        where book.Id == id
            //        select book).FirstOrDefault();

            // Query plus simple et beaucoup plus rapide
            Book = await _context.Books
                .Include(b => b.Category).Include(b => b.Genres).FirstOrDefaultAsync(m => m.Id == id);

            if (Book == null)
            {
                return NotFound();
            }

            BookCopies = await _context.BookCopies.Where(x => x.BookId == Book.Id).ToListAsync();
            return Page();
        }
    }
}
