﻿using DAWProiect.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DAWProiect.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var categories = from category in db.Categories
                             orderby category.CategoryName
                             select category;

            ViewBag.Categories = categories;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();
        }


        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            //ViewBag.Category = category;
            ViewBag.CategoryId = category.CategoryId;
            ViewBag.CategoryName = category.CategoryName;
            var userId = User.Identity.GetUserId();

            ViewBag.Allow = db.Roles.Any(x => x.Users.Any(y => y.UserId == userId) && x.Name == "Administrator");
            
            var articles = db.Articles.Include("Category").Include("User").Where(a => a.CategoryId == id);
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            ViewBag.Articles = articles;
            //return View(category);
            //return View(articles);
            return View();
        }

        [HttpPost]
        public ActionResult Show(int id, string sortType)
        {
            Category category = db.Categories.Find(id);
            //ViewBag.Category = category;
            ViewBag.CategoryId = category.CategoryId;
            ViewBag.CategoryName = category.CategoryName;
            var userId = User.Identity.GetUserId();

            ViewBag.Allow = db.Roles.Any(x => x.Users.Any(y => y.UserId == userId) && x.Name == "Administrator");
            var articles = db.Articles.Include("Category").Include("User").Where(a => a.CategoryId == id);
            
            if (sortType == "Title")
                articles = db.Articles.Include("Category").Include("User").Where(a => a.CategoryId == id).OrderBy(x => x.Title);
            //articles.OrderBy(x => x.Title);
            else 
            if(sortType == "Date")
                //articles.OrderByDescending(x => x.Date);
                articles = db.Articles.Include("Category").Include("User").Where(a => a.CategoryId == id).OrderByDescending(x => x.Date);


            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            ViewBag.Articles = articles;
            //return View(category);
            //return View(articles);
            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;
            return View(category);
        }

        [HttpPut]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = db.Categories.Find(id);
                    if (TryUpdateModel(category))
                    {
                        category.CategoryName = requestCategory.CategoryName;
                        db.SaveChanges();
                        TempData["message"] = "Categoria a fost modificata!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestCategory);
                }

            }
            catch (Exception e)
            {
                return View(requestCategory);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost stearsa!";
            return RedirectToAction("Index");
        }

        public void sortRecent(int id)
        {
            var articles = db.Articles.Include("Category").Include("User").Where(a => a.CategoryId == id).OrderByDescending(a => a.Date);
            ViewBag.Articles = articles;
        }
    }
}