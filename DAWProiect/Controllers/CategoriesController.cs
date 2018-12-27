﻿using DAWProiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            ViewBag.Category = category;

            return View(category);
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
    }
}