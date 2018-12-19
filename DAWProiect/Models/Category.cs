using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAWProiect.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Numele categoriei este obligatoriu!")]
        [Display(Name = "Nume Categorie")]
        public string CategoryName { get; set; }

        public virtual ICollection<Article> Article { get; set; }

    }
}