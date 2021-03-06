﻿using DAWProiect.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWProiect.Controllers
{
    public class ArticleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ActionResult Index()
        {
            var articles = db.Articles.Include("Category").Include("User");
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            ViewBag.Articles = articles;

            return View();
        }


        public ActionResult Show(int id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            Article article = db.Articles.Find(id);
            
            ViewBag.Article = article;
            ViewBag.Category = article.Category;

            var comments = db.Comments.Include("User").Include("Article").Where(x => x.ArticleId == article.Id).OrderByDescending(x => x.Date);
            ViewBag.Comments = comments;

            var userId = User.Identity.GetUserId();
            ViewBag.loggedInUser = userId;

            return View(article);

        }

        [HttpPost]
        public ActionResult AddCom(Comment comment)
        {
            if(String.IsNullOrEmpty(comment.Content))
            {
                return Redirect("Show/" + comment.ArticleId.ToString());
            }
            else
            {
                var userId = User.Identity.GetUserId();
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else comment.UserId = userId;
                comment.Date = DateTime.Now;

                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("Show/" + comment.ArticleId.ToString());
            }
        }

    
        [HttpDelete]
        public ActionResult DeleteComm(int id)
        {

            Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.Identity.GetUserId() == comm.Article.UserId)
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost stars!";
                return RedirectToAction("Show/" + comm.ArticleId.ToString());
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un comentariu care nu va apartine!";
                return RedirectToAction("Show/" + comm.ArticleId.ToString());
            }
        }

        [HttpPost]
        public ActionResult EditComm(int id, Comment requestComm)
        {
            Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator") || User.Identity.GetUserId() == comm.Article.UserId)
            {
                comm.Content = requestComm.Content;
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost modificat!";
                return RedirectToAction("Show/" + comm.ArticleId.ToString());
            }
            else
                return RedirectToAction("Show/" + comm.ArticleId.ToString());
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            Article article = new Article();
            // preluam lista de categorii din metoda GetAllCategories()
            article.Categories = GetAllCategories();
            return View(article);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New([Bind(Exclude = "ArticlePhoto")]Article article)
        {
            article.Categories = GetAllCategories();
            try
            {
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["ArticlePhoto"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }
                article.ArticlePhoto = imageData;

                _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var userId = User.Identity.GetUserId();
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else article.UserId = userId;

                if (ModelState.IsValid)
                {
                   
                   

                    db.Articles.Add(article);
                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost adaugat!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(article);
                }
            }
            catch (Exception e)
            {
                return View(article);
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
        {

            Article article = db.Articles.Find(id);
            ViewBag.Article = article;
            article.Categories = GetAllCategories();
            if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                return View(article);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei stiri care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id, [Bind(Exclude = "ArticlePhoto")]Article requestArticle)
        {
            Article article = db.Articles.Find(id);
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["ArticlePhoto"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }

                //UserPhoto is not updated if no file is chosen
                if (imageData.Length > 0)
                {
                    article.ArticlePhoto = imageData;
                }

            }
            try
            {
                if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
                {
                    article.Title = requestArticle.Title;
                    article.Content = requestArticle.Content;
                    article.Date = requestArticle.Date;
                    article.CategoryId = requestArticle.CategoryId;
                    db.SaveChanges();
                    TempData["message"] = "Stirea a fost modificata!";
                }
                    return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Find(id);
            if (article.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                db.Articles.Remove(article);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters!";
            }
            return RedirectToAction("Index");
        }

        public FileContentResult DisplayArticlePhoto(int artId)
        {
            
           var article = from art in db.Articles
                               where art.Id.Equals(artId)
                               select art;
            var artImage = article.FirstOrDefault().ArticlePhoto;
           
           if (artImage == null ||artImage.Length <= 0)
           {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");
            }

            return new FileContentResult(artImage, "image/jpeg");

        }

    }

}
