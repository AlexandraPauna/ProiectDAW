using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAWProiect.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [Display(Name = "Comentariu")]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ArticleId { get; set; }
        public virtual Article Article{ get; set; }
    }
}