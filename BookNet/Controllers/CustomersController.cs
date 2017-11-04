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
    [Authorization(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private BookStoreModel db = new BookStoreModel();

        // GET: Customers
        public ActionResult Index(string firstname, string lastname, string email, string city, string PhoneNumber)
        {
            var customers = from s in db.Customers
                            select s;

            if (!String.IsNullOrEmpty(firstname))
            {
                customers = customers.Where(s => s.FirstName.Contains(firstname));
            }

            if (!String.IsNullOrEmpty(lastname))
            {
                customers = customers.Where(s => s.LastName.Contains(lastname));
            }

            if (!String.IsNullOrEmpty(email))
            {
                customers = customers.Where(s => s.Email.ToString().Contains(email));
            }

            if (!String.IsNullOrEmpty(city))
            {
                customers = customers.Where(s => s.City.ToString().Contains(city));
            }

            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                customers = customers.Where(s => s.PhoneNumber.ToString().Contains(PhoneNumber));
            }

            return View(customers.ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Email,City,Street,PhoneNumber,BirthDate,CreationDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (db.Customers.Any(x => x.ID == customer.ID))
                {
                    ModelState.AddModelError("ID", "A customer with this ID already exist.");
                }
                else
                {
                    customer.CreationDate = DateTime.Now;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,City,Street,PhoneNumber,BirthDate,CreationDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
