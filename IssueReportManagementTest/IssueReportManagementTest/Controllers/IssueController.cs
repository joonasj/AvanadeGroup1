using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueReportManagementTest.Models;
using IssueReportManagementTest.ViewModel;
using System.Data.SqlClient;

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
        public ActionResult Update(FormCollection c)
        {
            //Issue ID
            int id = Convert.ToInt16(c["cissue.IssueID"]);
            //Old state
            int o_state = Convert.ToInt16(c["cissue.State"]);
            //new state
            int n_state = Convert.ToInt16(c["new-state"]);
            //Activity content
            string ac_content = c["new_activity"];
            //Server side validation
            /*if (id > 0 && o_state > 0 && n_state > 0)
            {*/
                //Current employee/adminitrator
                string c_employee = System.Web.HttpContext.Current.User.Identity.Name;
                //Day when activity has been added
                string mod = DateTime.Today.ToString("dd.MM.yyyy");
                //Automatic message if activity text is null
                if (ac_content == null)
                {
                    switch (n_state)
                    {
                        case 1:
                            ac_content = "Employee " + c_employee + " has started to work with current issue.";
                            break;
                        case 2:
                            ac_content = "Issue canno't be solved without at the moment. More information from " + c_employee + ".";
                            break;
                        case 3:
                            ac_content = "Issue has been resolved by " + c_employee + ".";
                            break;
                        case 4:
                            ac_content = "Issue has been archived by " + c_employee + ".";
                            break;
                        default:
                            ac_content = "Error has happened. Contact immediately to site administration.";
                            break;
                    }
                }
                string issue_query = "UPDATE Issue SET State='" + n_state + "', Modiefied='" + mod + "'";
                string activity_query = "INSERT INTO Activity (IssueID, Added, Employee, Context) VALUES ('" + id + "', '" + mod + "', '" + c_employee + "', '" + ac_content + "')";
                //db.Issues.SqlQuery(issue_query);
                //db.SaveChanges();
                //db.Activities.SqlQuery(activity_query);
                //db.Database.SqlQuery(issue_query);
                //db.Database.SqlQuery(activity_query);
                //System.Data.Entity.Database.SqlQuery();
                //db.Activities.S
                //SqlConnection cn = new SqlConnection("IRM.sdf");
                //cn.Open();
                SqlCommand icmd = new SqlCommand(issue_query);
                //icmd.Prepare();
                icmd.BeginExecuteNonQuery();
                //icmd.Transaction.Commit();
                //db.SaveChanges();
                SqlCommand acmd = new SqlCommand(activity_query);
                //return View(activity_query);
                return RedirectToAction("Index");

            //}
            /*else
            {
                return RedirectToAction("Error/1", "Home");
            }*/


            
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