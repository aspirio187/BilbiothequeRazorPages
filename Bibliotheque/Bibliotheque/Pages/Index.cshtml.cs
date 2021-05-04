using Bibliotheque.Data;
using Bibliotheque.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotheque.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public ICollection<Models.Book> Books { get; set; }

        public void OnGet()
        {
            Books = _context.Books.ToList();
        }
    }
}
