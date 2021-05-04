using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotheque.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Le titre est requis !")]
        [MaxLength(100, ErrorMessage = "Le titre doit faire au plus 100 caractères !")]
        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Required(ErrorMessage = "L'auteur est requis !")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "L'auteur doit faire entre {2} et {1} caractères !")]
        [Display(Name = "Auteur")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Le résumé est requis !")]
        [MinLength(50, ErrorMessage = "Le résumé doit faire au moins 50 caractères !")]
        [Display(Name = "Résumé")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Required(ErrorMessage = "La date de sortie est requise !")]
        [Display(Name = "Date de sortie")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "L'éditeur est requis !")]
        [MaxLength(25, ErrorMessage = "L'éditeur doit faire au plus 25 caractères !")]
        [Display(Name = "Editeur")]
        public string Editor { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Le format est requis !")]
        [MaxLength(25, ErrorMessage = "Le format doit faire au plus 25 caractères !")]
        [Display(Name = "Format")]
        public string Format { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Le nombre de pages doit être compris entre {1} et {2} !")]
        [Display(Name = "Pages")]
        public int Pages { get; set; }

        [StringLength(13, MinimumLength = 10, ErrorMessage = "L'EAN doit faire entre {2} et {1} caractères !")]
        public string EAN { get; set; }

        [StringLength(13, MinimumLength = 10, ErrorMessage = "L'ISBN doit faire entre {2} et {1} caractères !")]
        public string ISBN { get; set; }

        [Display(Name = "Préface")]
        public byte[] Preface { get; set; }

        /*******************************************************************/
        /************************* Les références **************************/
        /*******************************************************************/

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }

        /*******************************************************************/
        /************************ Listes références ************************/
        /*******************************************************************/

        public ICollection<Genre> Genres { get; set; }
    }
}
