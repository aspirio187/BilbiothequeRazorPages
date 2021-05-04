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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bibliotheque.Pages.Admin.Book
{
    [Authorize(Roles = Roles.ADMIN_ROLE)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        [BindProperty]
        public Models.Book Book { get; set; }

        [BindProperty]
        public int SelectedGenreId { get; set; }

        public List<SelectListItem> AvailableGenres { get; set; } = new();

        public ICollection<Models.BookCopy> BookCopies { get; set; }


        public IActionResult OnGet(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound($"L'id est inccorect : {id}");
            }
            Book = _context.Books.Include(x => x.Genres).FirstOrDefault(x => x.Id == id);
            if (Book is null)
            {
                return NotFound($"Il n'existe pas de livre avec l'id : {id}");
            }
            var genres = _context.Genres.Where(x => !x.Books.Contains(Book)).ToList();
            foreach (var genre in genres)
            {
                AvailableGenres.Add(new SelectListItem { Value = genre.Id.ToString(), Text = genre.Name });
            }
            BookCopies = _context.BookCopies.Where(x => x.BookId == Book.Id).ToList();
            return Page();
        }

        public IActionResult OnPostAddGenre()
        {
            if (SelectedGenreId == 0)
            {
                return RedirectToThisPage();
            }
            var genre = _context.Genres.Find(SelectedGenreId);
            if (genre is null)
            {
                return NotFound();
            }
            var book = _context.Books.Include(x => x.Genres).FirstOrDefault(x => x.Id == Book.Id);
            book.Genres.Add(genre);
            _context.Attach(book).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToThisPage();
        }

        public IActionResult OnPostDeleteGenre(int? id)
        {
            if (id is null || id == 0)
            {
                return RedirectToThisPage();
            }
            var genre = _context.Genres.Find(id);
            if (genre is null)
            {
                return NotFound();
            }
            var book = _context.Books.Include(x => x.Genres).FirstOrDefault(x => x.Id == Book.Id);
            if (book is null)
            {
                return NotFound();
            }
            book.Genres.Remove(genre);
            _context.Attach(book).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToThisPage();
        }

        public IActionResult OnPostDeleteCopy(int? id)
        {
            if (id is null || id == 0)
            {
                return RedirectToThisPage();
            }

            var copy = _context.BookCopies.Find(id);
            if (copy is null)
            {
                return NotFound();
            }
            _context.BookCopies.Remove(copy);
            _context.SaveChanges();
            return RedirectToThisPage();
        }

        public IActionResult OnPostUpdateGenre()
        {
            if (SelectedGenreId == 0)
            {
                return RedirectToPage("Edit", new { id = Book.Id });
            }
            return RedirectToThisPage();
        }

        public IActionResult OnPost(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToThisPage();
            }

            if (!_context.Categories.Any(x => x.Id == Book.CategoryId))
            {
                ModelState.AddModelError("Category", "La catégorie sélectionnée n'existe pas!");
                return RedirectToThisPage();
            }

            if(file is not null)
            {
                Book.Preface = FileHelper.ConvertToBytes(file);
            }
            var bookFromDb = _context.Books.Find(Book.Id);
            if (bookFromDb is null)
            {
                ModelState.AddModelError("Book", "Le livre sélectionné n'existe pas !");
                return RedirectToThisPage();
            }

            MapBook(Book, ref bookFromDb);
            try
            {
                _context.SaveChanges();
                return RedirectToPage("./Index");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private IActionResult RedirectToThisPage()
        {
            return RedirectToPage("Edit", new { id = Book.Id });
        }

        private bool MapBook(Models.Book from, ref Models.Book to)
        {
            to.Title = from.Title;
            to.Author = from.Author;
            to.CategoryId = from.CategoryId;
            to.EAN = from.EAN;
            to.Editor = from.Editor;
            to.Format = from.Format;
            to.ISBN = from.ISBN;
            to.Pages = from.Pages;
            if(from.Preface is not null || from.Preface.Length > 0)
            {
                to.Preface = from.Preface;
            }
            to.ReleaseDate = from.ReleaseDate;
            to.Summary = from.Summary;
            return true;
        }
    }
}
