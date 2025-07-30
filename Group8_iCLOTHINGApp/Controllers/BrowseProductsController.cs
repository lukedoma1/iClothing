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
    public class BrowseProductsController : Controller
    {
        private Group8_iCLOTHINGDBEntities db = new Group8_iCLOTHINGDBEntities();

        /*public ActionResult Index(string searchTerm, int? BrandId, int? DepartmentId, int? CategoryId)
        {
            // Populate dropdown lists
            ViewBag.BrandId = new SelectList(db.Brand.OrderBy(b => b.brandName), "brandID", "brandName");
            ViewBag.DepartmentId = new SelectList(db.Department.OrderBy(d => d.departmentName), "DepartmentId", "Name");
            ViewBag.CategoryId = new SelectList(db.Category.OrderBy(c => c.categoryName), "CategoryId", "Name");

            var products = db.Product.Include(p => p.Brand).AsQueryable(); // Assume similar includes for Department and Category

            if (!String.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.productName.Contains(searchTerm));
            }

            if (BrandId.HasValue)
            {
                products = products.Where(p => p.productBrandID == BrandId);
            }

    
            if (DepartmentId.HasValue)
            {
                products = products.Where(p => p.productID == DepartmentId);
            }

            if (CategoryId.HasValue)
            {
                products = products.Where(p => p.productID == CategoryId);
            }

            return View(products.ToList());
        }*/


        // GET: Products
        public ActionResult Index(string searchTerm, int? BrandId)
        {
            ViewBag.BrandId = new SelectList(db.Brand.OrderBy(b => b.brandName), "brandID", "brandName");

            var products = db.Product.Include(p => p.Brand).AsQueryable();

            if (!String.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.productName.Contains(searchTerm));
            }

            if (BrandId.HasValue)
            {
                products = products.Where(p => p.productBrandID == BrandId);
            }

            return View(products.ToList());
        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Search(string searchTerm, int? BrandId) // Ensure parameter name matches dropdown name
        {
            var products = db.Product.Where(p => p.productName.Contains(searchTerm));

            if (BrandId.HasValue)
            {
                products = products.Where(p => p.productBrandID == BrandId.Value); // Ensure correct property name for brand ID
            }

            return View("Index", products.ToList());
        }



        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.productBrandID = new SelectList(db.Brand, "brandID", "brandName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productID,productName,productDescription,productPrice,productQty,productBrand,productBrandID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productBrandID = new SelectList(db.Brand, "brandID", "brandName", product.productBrandID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.productBrandID = new SelectList(db.Brand, "brandID", "brandName", product.productBrandID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productID,productName,productDescription,productPrice,productQty,productBrand,productBrandID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productBrandID = new SelectList(db.Brand, "brandID", "brandName", product.productBrandID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
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
