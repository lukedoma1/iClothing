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
    public class UserCommentsController : Controller
    {
        private Group8_iCLOTHINGDBEntities db = new Group8_iCLOTHINGDBEntities();

        public ActionResult SeeAdminResponse()
        {
            var userComments = db.UserComments.Include("UserPassword").ToList();
            return View(userComments);
        }

        [HttpPost]
        public ActionResult SubmitResponse(int commentNo, string adminResponse)
        {
            var comment = db.UserComments.Find(commentNo);
            if (comment != null)
            {
                comment.AdminResponse = adminResponse;
                db.SaveChanges();
            }

            // Redirect back to the Admin Responses view
            return RedirectToAction("SeeAdminResponse");
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult SeeUserComments()
        {
            var userComments = db.UserComments.Include("UserPassword").ToList();
            return View(userComments);
        }
    


    // GET: UserComments
    public ActionResult Index()
        {
            var userComments = db.UserComments.Include(u => u.UserPassword);
            return View(userComments.ToList());
        }

        // GET: UserComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserComments userComments = db.UserComments.Find(id);
            if (userComments == null)
            {
                return HttpNotFound();
            }
            return View(userComments);
        }

        // GET: UserComments/Create
        public ActionResult Create()
        {
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName");
            return View();
        }

        // POST: UserComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "commentDescription")] UserComments userComments)
        {
            if (ModelState.IsValid)
            {
                userComments.customerID = db.UserPassword.Select(c => c.userID).FirstOrDefault();

                /*if (userComments.customerID == 0)
                {
                    ModelState.AddModelError("", "You need to be logged in to leave feedback.");
                    return View(userComments);
                }*/

                var maxCommentNo = db.UserComments.Any() ? db.UserComments.Max(uc => uc.commentNo) + 1 : 1;
                userComments.commentNo = maxCommentNo;
                userComments.commentDate = DateTime.Now;

                db.UserComments.Add(userComments);
                db.SaveChanges();

                ViewBag.SuccessMessage = "User comment created successfully.";

                return RedirectToAction("SeeUserComments");
            }


            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userComments.customerID);
            return View(userComments);
        }

        // GET: UserComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserComments userComments = db.UserComments.Find(id);
            if (userComments == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userComments.customerID);
            return View(userComments);
        }

        // POST: UserComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentNo,commentDate,commentDescription,customerID")] UserComments userComments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userComments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userComments.customerID);
            return View(userComments);
        }

        // GET: UserComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserComments userComments = db.UserComments.Find(id);
            if (userComments == null)
            {
                return HttpNotFound();
            }
            return View(userComments);
        }

        // POST: UserComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserComments userComments = db.UserComments.Find(id);
            db.UserComments.Remove(userComments);
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
