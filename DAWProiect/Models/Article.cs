﻿using System;
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

        [Required(ErrorMessage = "Introduceti titlul!")]
        [Display(Name = "Titlu Stire")]
        [StringLength(70, ErrorMessage = "Titlul este prea lung")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Introduceti continutul stirii!")]
        [Display(Name = "Continut Stire")]
        [MinLength(10, ErrorMessage = "Stirea este prea scurta! Va rugam adaugati detalii!")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Data Stire")]
        public DateTime Date { get; set; }

        [Display(Name = "Poza Stire")]
        public byte[] ArticlePhoto { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Selectati categoria!")]
        [Display(Name = "Categorie")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}