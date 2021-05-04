using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bibliotheque.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bibliotheque.Pages.Admin
{
    [Authorize(Roles = Roles.ADMIN_ROLE)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public ICollection<Models.Book> Books { get; set; }

        public void OnGet()
        {
            Books = _context.Books.Take(8).ToList();
        }
    }
}
