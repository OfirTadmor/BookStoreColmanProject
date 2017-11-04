using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookNet.Models;

namespace BookNet.Controllers
{
    public class AuthorsController : Controller
    {
        private BookStoreModel db = new BookStoreModel();

        // GET: Authors
        public ActionResult Index(string firstname, string lastname, Genre? specialty)
        {
            ViewBag.IsAdmin = AuthorizationAttribute.IsAdminLogedIn();

            var genreList = Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            genreList.ForEach(x => selectListItemList.Add(new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }));
            selectListItemList.Add(new SelectListItem { Text = "All", Value = null, Selected = true });

            ViewBag.SpecialtySelectList = selectListItemList;


            var authorsList = from s in db.Authors
                              select s;
            if (!String.IsNullOrEmpty(firstname))
            {
                authorsList = authorsList.Where(s => s.FirstName.Contains(firstname));
            }

            if (!String.IsNullOrEmpty(lastname))
            {
                authorsList = authorsList.Where(s => s.LastName.Contains(lastname));
            }

            if (specialty != null)
            {
                authorsList = authorsList.Where(s => s.Specialty == specialty);
            }

            return View(authorsList.ToList());
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.IsAdmin = (HttpContext.Session["userAuth"] != null && HttpContext.Session["userAuth"].ToString() == Roles.Admin.ToString());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        [Authorization(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Age,Image,Specialty")] Author author, HttpPostedFileBase imageUpload)
        {
            bool isFileValid = true;

            if (ModelState.IsValid)
            {
                if (imageUpload != null && imageUpload.ContentLength > 0)
                {
                    if (Utils.SaveImage(imageUpload))
                    {
                        isFileValid = true;
                        author.Image = imageUpload.FileName;
                    }
                    else
                    {
                        isFileValid = false;
                        ModelState.AddModelError("", "The author image could not been saved.");
                    }
                }

                if (isFileValid)
                {
                    db.Authors.Add(author);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        [Authorization(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Age,Image,Specialty")] Author author, HttpPostedFileBase imageUpload)
        {
            bool isFileValid = true;

            if (ModelState.IsValid)
            {
                if (imageUpload != null && imageUpload.ContentLength > 0)
                {
                    if (Utils.SaveImage(imageUpload))
                    {
                        isFileValid = true;
                        author.Image = imageUpload.FileName;
                    }
                    else
                    {
                        isFileValid = false;
                        ModelState.AddModelError("", "The new author image could not been saved.");
                    }
                }

                if (isFileValid)
                {
                    db.Entry(author).State = EntityState.Modified;

                    if (string.IsNullOrEmpty(author.Image))
                        db.Entry(author).Property(p => p.Image).IsModified = false;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        [Authorization(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorization(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
