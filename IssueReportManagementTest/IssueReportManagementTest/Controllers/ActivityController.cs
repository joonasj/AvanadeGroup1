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
    [Authorize]
    public class ActivityController : Controller
    {
        private IssueContext db = new IssueContext();

        //
        // GET: /Activity/
        [Authorize(Roles = "Administrator")]
        public ViewResult Index()
        {
            var activities = db.Activities.Include(a => a.Issue);
            return View(activities.ToList());
        }

        //
        // GET: /Activity/Details/5
        [Authorize(Roles = "Administrator")]
        public ViewResult Details(int id)
        {
            Activity activity = db.Activities.Find(id);
            return View(activity);
        }

        //
        // GET: /Activity/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.IssueID = new SelectList(db.Issues, "IssueID", "Title");
            return View();
        } 

        //
        // POST: /Activity/Create

        [HttpPost]
        public ActionResult Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.IssueID = new SelectList(db.Issues, "IssueID", "Title", activity.IssueID);
            return View(activity);
        }
        
        //
        // GET: /Activity/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Activity activity = db.Activities.Find(id);
            ViewBag.IssueID = new SelectList(db.Issues, "IssueID", "Title", activity.IssueID);
            return View(activity);
        }

        //
        // POST: /Activity/Edit/5

        [HttpPost]
        public ActionResult Edit(Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IssueID = new SelectList(db.Issues, "IssueID", "Title", activity.IssueID);
            return View(activity);
        }

        //
        // GET: /Activity/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            Activity activity = db.Activities.Find(id);
            return View(activity);
        }

        //
        // POST: /Activity/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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