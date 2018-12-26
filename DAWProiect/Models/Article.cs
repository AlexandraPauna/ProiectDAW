using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWProiect.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titlu Stire")]
        [StringLength(70, ErrorMessage = "Titlul este prea lung")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Continut Stire")]
        [MinLength(10, ErrorMessage = "Stirea este prea scurta! Va rugam adaugati detalii!")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Data Stire")]
        public DateTime Date { get; set; }

        [Display(Name = "Poza Stire")]
        public byte[] UserPhoto { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Va rugam selectati categoria!")]
        [Display(Name = "Categorie")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}