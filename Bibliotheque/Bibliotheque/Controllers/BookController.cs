using Bibliotheque.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotheque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [Route("Preface/{id}")]
        public IActionResult GetPreface(int? id)
        {
            try
            {
                if (id is null || id == 0)
                {
                    return BadRequest($"L'ID est incorrect : {id}");
                }

                byte[] preface = (from book in _context.Books
                                  where book.Id == id
                                  select book.Preface).FirstOrDefault();

                if (preface is not null && preface.Length > 0)
                {
                    return File(preface, "image/jpg");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
