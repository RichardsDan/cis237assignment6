using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237assignment6.Models;

namespace cis237assignment6.Controllers
{
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageDRichardsEntities db = new BeverageDRichardsEntities();

        // GET: Beverages
        public ActionResult Index()
        {
            // Setup a variable to hold the Beverages data set
            DbSet<Beverage> BeveragesToSearch = db.Beverages;

            // Setup some strings to hold the data that might be in the session.
            // If there is nothing in the session we can still use these variables
            // as a default value.
            string filterName = "";
            string filterMin = "";
            string filterMax = "";

            // Minimum and maximum prices
            decimal min = 0;
            decimal max = 100;

            // Check to see if there is a value in the session, and if there is,
            // assign it to the variable that we setup to hold the value.
            if (Session["name"] != null && !String.IsNullOrWhiteSpace((string)Session["name"]))
            {
                filterName = (string)Session["name"];
            }
            if (Session["min"] != null && !String.IsNullOrWhiteSpace((string)Session["min"]))
            {
                filterMin = (string)Session["min"];
                min = Decimal.Parse(filterMin);
            }
            if (Session["max"] != null && !String.IsNullOrWhiteSpace((string)Session["max"]))
            {
                filterMax = (string)Session["max"];
                max = Decimal.Parse(filterMax);
            }

            // Do the actual filter on the Beverages dataset
            IEnumerable<Beverage> filtered = BeveragesToSearch.Where(beverage => beverage.price >= min &&
                                                                                 beverage.price <= max &&
                                                                                 beverage.name.Contains(filterName));

            // Place the strings into the ViewBag so they can be displayed
            ViewBag.filterDesc = filterName;
            ViewBag.filterMin = filterMin;
            ViewBag.filterMax = filterMax;

            // Return the view with the filtered selection of beverages
            return View(filtered);
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Filter method. Takes data submitted from the form and
        // stores it in the session so it can be accessed later.
        public ActionResult Filter()
        {
            String name = Request.Form.Get("name");
            String min = Request.Form.Get("min");
            String max = Request.Form.Get("max");

            // Store the form data into the session so that it can be retrieved later
            Session["name"] = name;
            Session["min"] = min;
            Session["max"] = max;

            // Redirect the user to the index page. The filter work will be done in the Index
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
