using Bibliotheque.Data;
using Bibliotheque.Models;
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
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [Route("GetCategories")]
        public IActionResult GetCategories()
        {
            return Ok(_context.Categories.ToList());
        }

        [HttpGet]
        [Route("GetCategory/{id}")]
        public IActionResult GetCategory(int? id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound(id);
            }
            return Ok(category);
        }

        [HttpPost]
        [Route("AddCategory")]
        public IActionResult AddCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Add(model);
                    if (_context.SaveChanges() > 0)
                    {
                        return CreatedAtAction(nameof(GetCategory), new { id = model.Id }, model);
                    }
                    return NotFound(model);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteCategory/{id}")]
        public IActionResult DeleteCategory(int? id)
        {
            if (id is null || id == 0)
            {
                return BadRequest($"ID ne peut pas être null ou 0 : {id}");
            }
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound($"Il n'existe aucune catégorie avec l'ID : {id}");
            }
            _context.Categories.Remove(category);
            if (_context.SaveChanges() > 0)
            {
                return NoContent();
            }
            return BadRequest("La suppression a échouée");
        }
    }
}
