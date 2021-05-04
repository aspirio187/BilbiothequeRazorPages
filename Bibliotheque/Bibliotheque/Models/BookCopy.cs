using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotheque.Models
{
    public class BookCopy
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "L'état est requis !")]
        [MaxLength(50, ErrorMessage = "L'état doit faire au plus 50 caractères !")]
        [Display(Name = "Etat")]
        public string State { get; set; }

        [Range(0, 100, ErrorMessage = "La quantité doit être comprise entre {0} et {1}")]
        [Display(Name = "Quantité")]
        public int Quantity { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public int BookId { get; set; }
    }
}
