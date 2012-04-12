using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueReportManagementTest.Models;
using IssueReportManagementTest.ViewModel;

namespace IssueReportManagementTest.Controllers
{ 
    public class IssueController : Controller
    {
        private IssueContext db = new IssueContext();

        //
        // GET: /Issue/

        public ViewResult Index()
        {
            var issues = db.Issues.Include(i => i.Category).Include(i => i.Priority);
            return View(issues.ToList());
        }

        //
        // GET: /Issue/Details/5

        public ViewResult Details(int id)
        {
            Issue issue = db.Issues.Find(id);
            string current_state;
            switch (issue.State)
            {
                case 0:
                    current_state = "Not started";
                    break;
                case 1:
                    current_state = "Started";
                    break;
                case 2:
                    current_state = "Waiting";
                    break;
                case 3:
                    current_state = "Resolved";
                    break;
                case 4:
                    current_state = "Closed";
                    break;
                default:
                    current_state = "Error";
                    break;
            }
            var viewModel = new IssueViewModel
            {
                cissue = issue,
                cstate = current_state,
                cactivities = db.Activities.SqlQuery("SELECT * FROM Activity WHERE IssueID='"+id+"'")
                
            };

            return View(viewModel);
        }

        //
        // GET: /Issue/Create

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.PriorityID = new SelectList(db.Priorities, "ID", "Name");
            return View();
        } 

        //
        // POST: /Issue/Create

        [HttpPost]
        public ActionResult Create(Issue issue)
        {
            issue.Writer = System.Web.HttpContext.Current.User.Identity.Name;
            if (ModelState.IsValid)
            {
                db.Issues.Add(issue);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", issue.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priorities, "ID", "Name", issue.PriorityID);
            return View(issue);
        }
        
        //
        // GET: /Issue/Edit/5
 
        public ActionResult Edit(int id)
        {
            Issue issue = db.Issues.Find(id);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", issue.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priorities, "ID", "Name", issue.PriorityID);
            return View(issue);
        }

        //
        // POST: /Issue/Edit/5

        [HttpPost]
        public ActionResult Edit(Issue issue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", issue.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priorities, "ID", "Name", issue.PriorityID);
            return View(issue);
        }

        [HttpPost]
        public ActionResult Update()
        {
            string ac_content = Request['a'];
            int id = System.Web.HttpRequest.['b'];
            return RedirectToAction("Index");
        }

        //
        // GET: /Issue/Delete/5
 
        public ActionResult Delete(int id)
        {
            Issue issue = db.Issues.Find(id);
            return View(issue);
        }

        //
        // POST: /Issue/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Issue issue = db.Issues.Find(id);
            db.Issues.Remove(issue);
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