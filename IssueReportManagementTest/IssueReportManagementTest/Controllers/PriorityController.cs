using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueReportManagementTest.Models;

namespace IssueReportManagementTest.Controllers
{ 
    public class PriorityController : Controller
    {
        private IssueContext db = new IssueContext();

        //
        // GET: /Priority/

        public ViewResult Index()
        {
            return View(db.Priorities.ToList());
        }

        //
        // GET: /Priority/Details/5

        public ViewResult Details(int id)
        {
            Priority priority = db.Priorities.Find(id);
            return View(priority);
        }

        //
        // GET: /Priority/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Priority/Create

        [HttpPost]
        public ActionResult Create(Priority priority)
        {
            if (ModelState.IsValid)
            {
                db.Priorities.Add(priority);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(priority);
        }
        
        //
        // GET: /Priority/Edit/5
 
        public ActionResult Edit(int id)
        {
            Priority priority = db.Priorities.Find(id);
            return View(priority);
        }

        //
        // POST: /Priority/Edit/5

        [HttpPost]
        public ActionResult Edit(Priority priority)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priority).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(priority);
        }

        //
        // GET: /Priority/Delete/5
 
        public ActionResult Delete(int id)
        {
            Priority priority = db.Priorities.Find(id);
            return View(priority);
        }

        //
        // POST: /Priority/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Priority priority = db.Priorities.Find(id);
            db.Priorities.Remove(priority);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}