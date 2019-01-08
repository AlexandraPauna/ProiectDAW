using DAWProiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWProiect.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string searchTitle)
        {
            var articles = db.Articles.Include("Category").Include("User").OrderByDescending(a => a.Date);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            ViewBag.Articles = articles;

            if (!String.IsNullOrEmpty(searchTitle))
            {
                ViewBag.Articles = articles.Where(s => s.Title.Contains(searchTitle));
            }

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}