using BookNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BookNet.Controllers
{    
    public class AdminController : Controller
    {
        [Authorization(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (AuthorizationAttribute.IsAdminLogedIn())
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        
        public ActionResult Logout()
        {
            HttpContext.Session["userAuth"] = null;
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                string username = form["Username"] ?? string.Empty;
                string password = form["Password"] ?? string.Empty;

                if (username == Properties.Settings.Default.AdminUser &&
                    password == Properties.Settings.Default.AdminPass)
                {
                    HttpContext.Session.Add("userAuth", Roles.Admin.ToString());
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "The username or password was incorrect.");
            return View();
        }
        
        [Authorization(Roles = "Admin")]
        public ActionResult BooksReport()
        {
            using (BookStoreModel db = new BookStoreModel())
            {                
                var booksList = (from book in db.Books
                                 group book by book.Genre into g
                                 select g).ToList();                                 

                return View(booksList);
            }            
        }

        [Authorization(Roles = "Admin")]
        public int GetBookCustomers(int bookId)
        {
            using (BookStoreModel db = new BookStoreModel())
            {
                return db.Books.Where(x => x.ID == bookId).Include(p => p.Customers).First().Customers.Count;
            }
        }
    }
}