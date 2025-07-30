using Group8_iCLOTHINGApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group8_iCLOTHINGApp.Controllers
{
    public class HomeController : Controller
    {
        private Group8_iCLOTHINGDBEntities db = new Group8_iCLOTHINGDBEntities();

        public ActionResult WelcomeAdmin()
        {
            return View();
        }
        public ActionResult WelcomeUser()
        {
            return View();
        }
        public ActionResult AboutAdmin()
        {
            return View(db.AboutUs.ToList());
        }
        public ActionResult AboutSignedIn()
        {
            return View(db.AboutUs.ToList());
        }
        public ActionResult ContactSignedIn()
        {
            return View();
        }
        public ActionResult LoginAdmin()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserPassword objUser)
        {
            if (ModelState.IsValid)
            {
                using (Group8_iCLOTHINGDBEntities db = new Group8_iCLOTHINGDBEntities())
                {
                    var obj = db.UserPassword.Where(a => a.userAccountName.Equals(objUser.userAccountName) && a.userEncryptedPassword.Equals(objUser.userEncryptedPassword)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.userID.ToString();
                        Session["UserName"] = obj.userAccountName.ToString();
                        return RedirectToAction("WelcomeUser");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View(db.AboutUs.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}