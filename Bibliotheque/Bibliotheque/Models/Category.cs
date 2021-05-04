using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotheque.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis !")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Le nom doit faire entre {2} et {1} caractères !")]
        [Display(Name = "Nom")]
        public string Name { get; set; }
    }
}
