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
    public class UserQueriesController : Controller
    {
        private Group8_iCLOTHINGDBEntities db = new Group8_iCLOTHINGDBEntities();

        // GET: UserQueries
        public ActionResult Index()
        {
            return View(db.UserQuery.ToList());
        }

        // GET: UserQueries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuery userQuery = db.UserQuery.Find(id);
            if (userQuery == null)
            {
                return HttpNotFound();
            }
            return View(userQuery);
        }

        // GET: UserQueries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserQueries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "queryDescription")] UserQuery userQuery)
        {
            if (ModelState.IsValid)
            {
                var maxQueryNo = db.UserQuery.Any() ? db.UserQuery.Max(uq => uq.queryNo) + 1 : 0;
                userQuery.queryNo = maxQueryNo;
                userQuery.queryDate = DateTime.Now;
                db.UserQuery.Add(userQuery);
                db.SaveChanges();
                ViewBag.SuccessMessage = "Your query has been successfully submitted.";
                return View(new UserQuery());
            }

            return View(userQuery);
        }

        // GET: UserQueries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuery userQuery = db.UserQuery.Find(id);
            if (userQuery == null)
            {
                return HttpNotFound();
            }
            return View(userQuery);
        }

        // POST: UserQueries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "queryNo,queryDate,queryDescription")] UserQuery userQuery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userQuery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userQuery);
        }

        // GET: UserQueries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserQuery userQuery = db.UserQuery.Find(id);
            if (userQuery == null)
            {
                return HttpNotFound();
            }
            return View(userQuery);
        }

        // POST: UserQueries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserQuery userQuery = db.UserQuery.Find(id);
            db.UserQuery.Remove(userQuery);
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
