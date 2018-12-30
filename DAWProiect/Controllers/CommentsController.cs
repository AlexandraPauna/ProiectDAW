using DAWProiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DAWProiect.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Create(int articleID)
        {
            var newComment = new Comment();
            newComment.ArticleId = articleID; // this will be sent from the ArticleDetails View, hold on :).

            return View(newComment);
        }

        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            try
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost adaugat!";
                return RedirectToAction("Show", "Article", new { comment.ArticleId});
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}