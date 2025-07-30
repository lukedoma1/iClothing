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
    public class ShoppingCartsController : Controller
    {
        private Group8_iCLOTHINGDBEntities db = new Group8_iCLOTHINGDBEntities();

        //imported from roberto's controllers
        public ActionResult AddToCart(int cartID, int cartProductID, int cartProductQty, double cartProductPrice)
        {

            var existingItem = db.ShoppingCart.FirstOrDefault(s => s.cartProductID == cartProductID);

            if (existingItem != null)
            {
                TempData["ErrorMessage"] = "Item is already in the shopping cart.";
                return RedirectToAction("Index", "BrowseProducts");
            }

            ShoppingCart shoppingCart = new ShoppingCart

            {
                cartID = cartID,
                cartProductID = cartProductID,
                cartProductQty = cartProductQty,
                cartProductPrice = cartProductPrice,
            };

            db.ShoppingCart.Add(shoppingCart);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: ShoppingCarts
        public ActionResult Index()
        {
            var shoppingCart = db.ShoppingCart.Include(s => s.Product);
            return View(shoppingCart.ToList());
        }

        // GET: ShoppingCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public ActionResult Create()
        {
            ViewBag.cartProductID = new SelectList(db.Product, "productID", "productName");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cartID,cartProductPrice,cartProductQty,cartProductID")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                //increment ID
                int maxUserID = db.ShoppingCart.Max(u => (int?)u.cartID) ?? 0; // Handle potential null values
                shoppingCart.cartID = maxUserID + 1;



                db.ShoppingCart.Add(shoppingCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cartProductID = new SelectList(db.Product, "productID", "productName", shoppingCart.cartProductID);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            ViewBag.cartProductID = new SelectList(db.Product, "productID", "productName", shoppingCart.cartProductID);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cartID,cartProductPrice,cartProductQty,cartProductID")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cartProductID = new SelectList(db.Product, "productID", "productName", shoppingCart.cartProductID);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            db.ShoppingCart.Remove(shoppingCart);
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
