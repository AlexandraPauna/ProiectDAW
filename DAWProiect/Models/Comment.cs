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
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Display(Name = "Comentariu")]
        public string Content { get; set; }
        public int ArticleId { get; set; }
        public virtual Article Article{ get; set; }
    }
}