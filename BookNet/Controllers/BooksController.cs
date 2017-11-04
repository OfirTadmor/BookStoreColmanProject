using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookNet.Models;
using BookNet.ConverterService;
using System.IO;

namespace BookNet.Controllers
{
    public class BooksController : Controller
    {
        const string DOLLAR_CURRENCY_CODE = "USD";
        const string SHEKEL_CURRENCY_CODE = "ILS";
        private BookStoreModel db = new BookStoreModel();

        public ActionResult Index(string authorname, string titleSearch, string genre)
        {
            ViewBag.IsAdmin = AuthorizationAttribute.IsAdminLogedIn();            

            var BookList = from s in db.Books.Include(b => b.Author)
                           select s;

            if (!String.IsNullOrEmpty(authorname))
            {
                BookList = BookList.Where(s => s.Author.LastName.Contains(authorname) || s.Author.FirstName.Contains(authorname));
            }

            if (!String.IsNullOrEmpty(titleSearch))
            {
                BookList = BookList.Where(s => s.Title.Contains(titleSearch));
            }

            if (!String.IsNullOrEmpty(genre))
            {
                BookList = BookList.Where(s => s.Genre.ToString().Contains(genre));
            }

            return View(BookList.ToList());
        }

        public ActionResult Details(int? id)
        {
            ViewBag.IsAdmin = AuthorizationAttribute.IsAdminLogedIn();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Where(x => x.ID == id).Include(p => p.Author).First();

            if (book == null)
            {
                return HttpNotFound();
            } 
            
            return View(book);
        }

        public ActionResult BuyBook(int? bookId)
        {
            if (bookId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.BookId = bookId;

            var book = db.Books.Find(bookId);
            ViewBag.BookName = "";
            ViewBag.BookPrice = "";

            if (book != null)
            {
                ViewBag.BookName = book.Title;
                ViewBag.BookPrice = book.Price;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyBook([Bind(Include = "ID,FirstName,LastName,Email,City,Street,PhoneNumber,BirthDate")] Customer customer, int? bookId)
        {
            if (ModelState.IsValid)
            {
                if (bookId != null)
                {
                    var book = db.Books.Where(x => x.ID == bookId).Include(path => path.Customers).First();

                    if (book != null)
                    {
                        var cus = db.Customers.Find(customer.ID);

                        if (cus != null)
                        {
                            if (!book.Customers.Any(x => x.ID == cus.ID))
                            {
                                book.Customers.Add(cus);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            customer.CreationDate = DateTime.Now.Date;
                            customer.BirthDate = customer.BirthDate.Date;
                            book.Customers.Add(customer);
                            db.SaveChanges();
                        }
                        
                        return RedirectToAction("Index");
                    }
                }
            }

            return RedirectToAction("BuyBook", new { bookId = bookId});
        }

        public decimal GetConversionRate()
        {
            decimal dConversionRate;

            ConverterSoapClient converterService = new ConverterSoapClient();
            dConversionRate = converterService.GetConversionRate(DOLLAR_CURRENCY_CODE,
                                                                 SHEKEL_CURRENCY_CODE,
                                                                 new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

            return (dConversionRate);
        }

        public PartialViewResult Aside()
        {
            var genreList = Enum.GetValues(typeof(Genre)).Cast<Genre>();
            return PartialView("~/Views/Partials/Categories.cshtml", genreList);
        }
               
        [Authorization(Roles = "Admin")]
        public ActionResult Create()
        {
            var authors = db.Authors.Select(author => new
            {
                ID = author.ID,
                FullName = author.FirstName + " " + author.LastName
            });

            ViewBag.AuthorID = new SelectList(authors, "ID", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Genre,Price,Image,AuthorID")] Book book, HttpPostedFileBase imageUpload)
        {
            bool isFileValid = true;

            if (ModelState.IsValid)
            {
                if (imageUpload != null && imageUpload.ContentLength > 0)
                {
                    if (Utils.SaveImage(imageUpload))
                    {
                        isFileValid = true;
                        book.Image = imageUpload.FileName;                        
                    }
                    else
                    {
                        isFileValid = false;
                        ModelState.AddModelError("", "The book image could not been saved.");
                    }                    
                }

                if (isFileValid)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FirstName", book.AuthorID);
            return View(book);
        }
        
        [Authorization(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            var authors = db.Authors.Select(author => new
            {
                ID = author.ID,
                FullName = author.FirstName + " " + author.LastName
            });

            ViewBag.AuthorID = new SelectList(authors, "ID", "FullName", book.AuthorID);
            return View(book);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorization(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Genre,Price,Image,AuthorID")] Book book, HttpPostedFileBase imageUpload)
        {
            bool isFileValid = true;

            if (ModelState.IsValid)
            {
                if (imageUpload != null && imageUpload.ContentLength > 0)
                {
                    if (Utils.SaveImage(imageUpload))
                    {
                        isFileValid = true;
                        book.Image = imageUpload.FileName;                        
                    }
                    else
                    {
                        isFileValid = false;
                        ModelState.AddModelError("", "The new book image could not been saved");
                    }                    
                }                

                if (isFileValid)
                {
                    db.Entry(book).State = EntityState.Modified;

                    if (string.IsNullOrEmpty(book.Image))
                        db.Entry(book).Property(p => p.Image).IsModified = false;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "FirstName", book.AuthorID);
            return View(book);
        }
                
        [Authorization(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Where(currBook => currBook.ID == id).Include(currBook => currBook.Author).First();
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
                
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorization(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
