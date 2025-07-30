using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group8_iCLOTHINGApp.Models;

namespace Group8_iCLOTHINGApp.Controllers
{
    public class UserPasswordsController : Controller
    {
        private Group8_iCLOTHINGDBEntities db = new Group8_iCLOTHINGDBEntities();

        /*// GET: UserPasswords/SignIn
        public ActionResult CreateAccount()
        {
            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName");
            return View();
        }*/

        // POST: UserPasswords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "userID,userAccountName,userEncryptedPassword,passwordExpiryTime,userAccountExpiryDate")] UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                if (db.UserPassword.Any(u => u.userAccountName == userPassword.userAccountName))
                {
                    ModelState.AddModelError("userAccountName", "Username already exists. Please choose a different username.");
                    ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName", userPassword.userID);
                    return View(userPassword);
                }

                // Check if the username is too long
                if (userPassword.userAccountName.Length > 14)
                {
                    ModelState.AddModelError("userAccountName", $"Username is too long. Maximum length allowed is {14} characters.");
                    ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName", userPassword.userID);
                    return View(userPassword);
                }

                //increment ID
                int maxUserID = db.UserPassword.Max(u => (int?)u.userID) ?? 0; // Handle potential null values
                userPassword.userID = maxUserID + 1;

                userPassword.passwordExpiryTime = 100;
                userPassword.userAccountExpiryDate = DateTime.Now.AddDays(userPassword.passwordExpiryTime);

                db.UserPassword.Add(userPassword);
                db.SaveChanges();
                return RedirectToAction("Login", "Home");
            }
            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName", userPassword.userID);
            return View(userPassword);
        }

        // GET: UserPasswords/SignIn
        public ActionResult SignIn()
        {
            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName");
            return View();
        }

        // POST: UserPasswords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "userID,userAccountName,userEncryptedPassword,passwordExpiryTime,userAccountExpiryDate")] UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                //increment ID
                int maxUserID = db.UserPassword.Max(u => (int?)u.userID) ?? 0; // Handle potential null values
                userPassword.userID = maxUserID + 1;

                userPassword.passwordExpiryTime = 100;
                userPassword.userAccountExpiryDate = DateTime.Now.AddDays(userPassword.passwordExpiryTime);

                db.UserPassword.Add(userPassword);
                db.SaveChanges();
                return RedirectToAction("SignIn");
            }

            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName", userPassword.userID);
            return View(userPassword);
        }

        // GET: UserPasswords
        public ActionResult Index()
        {
            /*var userPassword = db.UserPassword.Include(u => u.Customer);
            return View(userPassword.ToList());*/
            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName");
            return View();
        }

        // GET: UserPasswords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPassword.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            return View(userPassword);
        }

        // GET: UserPasswords/Create
        public ActionResult Create()
        {
            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName");
            return View();
        }

        // POST: UserPasswords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,userAccountName,userEncryptedPassword,passwordExpiryTime,userAccountExpiryDate")] UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                db.UserPassword.Add(userPassword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName", userPassword.userID);
            return View(userPassword);
        }

        // GET: UserPasswords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPassword.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName", userPassword.userID);
            return View(userPassword);
        }

        // POST: UserPasswords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,userAccountName,userEncryptedPassword,passwordExpiryTime,userAccountExpiryDate")] UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userPassword).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userID = new SelectList(db.Customer, "customerID", "customerName", userPassword.userID);
            return View(userPassword);
        }

        // GET: UserPasswords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPassword.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            return View(userPassword);
        }

        // POST: UserPasswords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserPassword userPassword = db.UserPassword.Find(id);
            db.UserPassword.Remove(userPassword);
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
