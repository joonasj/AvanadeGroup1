﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueReportManagementTest.Models;
using IssueReportManagementTest.ViewModel;
using System.Data.SqlClient;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;

namespace IssueReportManagementTest.Controllers
{ 
    [Authorize]
    public class IssueController : Controller
    {
        private IssueContext db = new IssueContext();
        private SmtpClient smtps = new SmtpClient("smtp.gmail.com", 587);
        //private string csvdata;

        //
        // GET: /Issue/
        [Authorize(Roles = "Administrator, Employee, Customer")]
        public ViewResult Index(string mode)
        {

            //var issues = db.Issues.Include(i => i.Category).Include(i => i.Priority);
            //string query = "SELECT * FROM Issue WHERE Writer='" + System.Web.HttpContext.Current.User.Identity.Name + "' ORDER BY State ASC";
            string query = "";
            string a_query = "";//for "closed" issues
            string e_query = ""; //for employee issues
            //ViewBag.Message = "Welcome to ASP.NET MVC!";
            string cuser = System.Web.HttpContext.Current.User.Identity.Name;
            if (System.Web.HttpContext.Current.User.IsInRole("Customer"))
            {
                

                if (mode != null)
                {
                    switch (mode)
                    {
                        case "id":
                            query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State<'4' ORDER BY IssueID DESC";
                            a_query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State='4' ORDER BY IssueID DESC";
                            break;
                        case "state":
                            query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State<'4' ORDER BY State ASC";
                            a_query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State='4' ORDER BY State ASC";
                            break;
                        case "cat":
                            query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State<'4' ORDER BY CategoryID ASC";
                            a_query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State='4' ORDER BY CategoryID ASC";
                            break;
                        case "priority":
                            query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State<'4' ORDER BY PriorityID DESC";
                            a_query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State='4' ORDER BY PriorityID DESC";
                            break;
                        case "title":
                            query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State<'4' ORDER BY Title ASC";
                            a_query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State='4' ORDER BY Title ASC";
                            break;
                        default:
                            query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State<'4' ORDER BY State ASC";
                            a_query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State='4' ORDER BY State ASC";
                            break;
                    }
                }
                else
                {
                    query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State<'4' ORDER BY State ASC";
                    a_query = "SELECT * FROM Issue WHERE Writer='" + cuser + "' AND State='4' ORDER BY State ASC";
                }
                var listIssueviewModel = new IssueListViewModel
                {
                    lissue = db.Issues.SqlQuery(query),
                    a_lissue = db.Issues.SqlQuery(a_query)
                };
                return View(listIssueviewModel);
            }
            else
            {
                if (mode != null)
                {
                    switch (mode)
                    {
                        case "id":
                            query = "SELECT * FROM Issue WHERE State<'4' ORDER BY IssueID DESC";
                            a_query = "SELECT * FROM Issue WHERE State='4' ORDER BY IssueID DESC";
                            e_query = "SELECT * FROM Issue WHERE State<'4' AND Employee='" + cuser + "' ORDER BY IssueID DESC";
                            break;
                        case "state":
                            query = "SELECT * FROM Issue WHERE State<'4' ORDER BY State ASC";
                            a_query = "SELECT * FROM Issue WHERE State='4' ORDER BY State ASC";
                            e_query = "SELECT * FROM Issue WHERE State<'4' AND Employee='"+cuser+"' ORDER BY State ASC";
                            break;
                        case "cat":
                            query = "SELECT * FROM Issue WHERE State<'4' ORDER BY CategoryID ASC";
                            a_query = "SELECT * FROM Issue WHERE State='4' ORDER BY CategoryID ASC";
                            e_query = "SELECT * FROM Issue WHERE State<'4' AND Employee='" + cuser + "' ORDER BY CategoryID ASC";
                            break;
                        case "priority":
                            query = "SELECT * FROM Issue WHERE State<'4' ORDER BY PriorityID DESC";
                            a_query = "SELECT * FROM Issue WHERE State='4' ORDER BY PriorityID DESC";
                            e_query = "SELECT * FROM Issue WHERE State<'4' AND Employee='" + cuser + "' ORDER BY PriorityID ASC";
                            break;
                        case "title":
                            query = "SELECT * FROM Issue WHERE State<'4' ORDER BY Title ASC";
                            a_query = "SELECT * FROM Issue WHERE State='4' ORDER BY Title ASC";
                            e_query = "SELECT * FROM Issue WHERE State<'4' AND Employee='" + cuser + "' ORDER BY Title ASC";
                            break;
                        default:
                            query = "SELECT * FROM Issue WHERE State<'4' ORDER BY State ASC";
                            a_query = "SELECT * FROM Issue WHERE State='4' ORDER BY State ASC";
                            e_query = "SELECT * FROM Issue WHERE State<'4' AND Employee='" + cuser + "' ORDER BY State ASC";
                            break;
                    }
                }
                else
                {
                    query = "SELECT * FROM Issue WHERE State<'4' ORDER BY State ASC";
                    a_query = "SELECT * FROM Issue WHERE State='4' ORDER BY State ASC";
                    e_query = "SELECT * FROM Issue WHERE State<'4' AND Employee='" + cuser + "' ORDER BY State ASC";
                }
                var listIssueviewModel = new IssueListViewModel
                {
                    lissue = db.Issues.SqlQuery(query),
                    a_lissue = db.Issues.SqlQuery(a_query),
                    e_lissue = db.Issues.SqlQuery(e_query)
                };
                return View(listIssueviewModel);

            }
            
            //return View(issues.ToList());
        }

        //
        // GET: /Issue/Details/5
        [Authorize(Roles = "Administrator, Employee, Customer")]
        public ActionResult Details(int id)
        {
            Issue issue = db.Issues.Find(id);
            //check that customer cannot read other issues
            if (System.Web.HttpContext.Current.User.IsInRole("Customer"))
            {
                if (issue.Writer != System.Web.HttpContext.Current.User.Identity.Name)
                {
                    return RedirectToAction("Index", "Issue");
                }
            }
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
        [Authorize(Roles = "Administrator, Employee, Customer")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.PriorityID = new SelectList(db.Priorities, "ID", "Name");
            return View();
        } 

        //
        // POST: /Issue/Create

        /*[HttpPost]
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
        }*/

        [HttpPost]
        public ActionResult Create(FormCollection c, HttpPostedFileBase IssueFile)
        {
            Issue issue = new Issue();
            issue.Title = c["Title"];
            issue.Added = System.DateTime.Today;
            issue.Modiefied = System.DateTime.Today;
            issue.PriorityID = System.Convert.ToInt16(c["PriorityID"]);
            issue.CategoryID = System.Convert.ToInt16(c["CategoryID"]);
            issue.Description = c["Description"];
            issue.State = 0;

            //HttpPostedFileBase attachment = c["IssueFile"];
            if (IssueFile != null)
            {
                if (IssueFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(IssueFile.FileName);
                    var path = Path.Combine(Server.MapPath("../Content/uploads"), fileName);
                    IssueFile.SaveAs(path);
                    issue.IssueFileURL = "../Content/uploads/" + fileName;
                    //Type file_ext = IssueFile.GetType();
                    issue.IssueFileExtension = IssueFile.ContentType;

                }
                else
                {
                    issue.IssueFileExtension = "";
                    issue.IssueFileURL = "";
                }
            }
            else
            {
                issue.IssueFileURL = "";
                issue.IssueFileExtension = "";
            }

            issue.Writer = System.Web.HttpContext.Current.User.Identity.Name;
            //issue.IssueFileURL = IssueFile.FileName;
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
        [Authorize(Roles = "Administrator, Employee")]
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
            //send information to customer
            string send_email = c["c-email"];
            //Employees
            string old_employee = c["cissue.Employee"];
            string employee = c["assign"];
            if (System.Web.HttpContext.Current.User.IsInRole("Employee")) 
            {
                employee = old_employee;
            }
            //Issue ID
            int id = Convert.ToInt16(c["cissue.IssueID"]);
            //Old state
            int o_state = Convert.ToInt16(c["cissue.State"]);
            //new state
            int n_state = Convert.ToInt16(c["new-state"]);
            //Activity content
            string ac_content = c["new_activity"];
            //Changes for employee assignment
            string assignment_str = "";
            if (old_employee != employee)
            {
                if (employee != "") {
                    //Send email notification
                    MembershipUser cms = Membership.GetUser(employee);
                    //employee info
                    MembershipUser ems = Membership.GetUser(System.Web.HttpContext.Current.User.Identity.Name);
                    //mail(from, to)
                    MailMessage e_msg = new MailMessage(ems.Email, cms.Email);
                    //Subject
                    e_msg.Subject = "IRM: Issue " + id + " has been assigned to you";
                    //String for domain and port
                    string issueURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port) + "/Issue/Details/" + id;

                    //Body
                    e_msg.Body = "Issue " + id + "has been assigned to you.\nLink to issue "+issueURL+"\n";

                    //Definitions

                    //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                    smtps.UseDefaultCredentials = false;
                    smtps.Credentials = new NetworkCredential("avanadg1@gmail.com", "3l173h4x");
                    smtps.EnableSsl = true;
                    //Send
                    try
                    {
                        smtps.Send(e_msg);
                    }
                    catch { 
                    }
                }

                if (old_employee == "" || old_employee == null)
                {
                    if (employee == "" || employee == null)
                    {
                        assignment_str = "This issue isn't assigned to anyone.<br />\n";
                    }
                    else
                    {
                        assignment_str = "This issue has been assigned to " + employee + ".<br />\n";
                    }
                }
                else
                {
                    if (employee == "" || employee == null)
                    {
                        assignment_str = "This issue isn't assigned to anyone.<br />\n";
                    }
                    else
                    {
                        assignment_str = "This issue has been assigned to " + employee + " from " + old_employee + ".<br />\n";
                    }
                }
            }
            
            
            //Changes to status
            string status_str = "";
            string parsed_content = "";
            
            if (o_state != n_state)
            {
                switch (o_state)
                {
                    case 0:
                        status_str = "Changed state from Not started to ";
                        break;
                    case 1:
                        status_str = "Changed state from Started to ";
                        break;
                    case 2:
                        status_str = "Changed state from Waiting to ";
                        break;
                    case 3:
                        status_str = "Changed state from Resolved to ";
                        break;
                    case 4:
                        status_str = "Changed state from Closed to ";
                        break;
                    default:
                        status_str = "";
                        break;
                }
                switch (n_state)
                {
                    case 1:
                        status_str = status_str + "Started.<br />\n";
                        break;
                    case 2:
                        status_str = status_str + "Waiting.<br />\n";
                        break;
                    case 3:
                        status_str = status_str + "Resolved.<br />\n";
                        break;
                    case 4:
                        status_str = status_str + "Closed.<br />\n";
                        break;
                    default:
                        status_str = "";
                        break;
                }

            }
            //Server side validation
         
                //Current employee/adminitrator
                string c_employee = System.Web.HttpContext.Current.User.Identity.Name;
                //Day when activity has been added
                string mod = DateTime.Today.ToString("dd.MM.yyyy");
                //Automatic message if activity text is null
                if (ac_content == null || ac_content == "")
                {
                    switch (n_state)
                    {
                        case 1:
                            ac_content = "Employee " + c_employee + " has started to work with current issue.";
                            break;
                        case 2:
                            ac_content = "Issue canno't be solved at the moment. More information from " + c_employee + ".";
                            break;
                        case 3:
                            ac_content = "Issue has been resolved by " + c_employee + ".";
                            break;
                        case 4:
                            ac_content = "Issue has been archived by " + c_employee + ".";
                            
                            //System.Web.HttpContext.Current.User.Identity.
                            break;
                        default:
                            ac_content = "Error has happened. Contact immediately to site administration.";
                            break;
                    }
                }
                
                //parse the message
                parsed_content = assignment_str + status_str + ac_content;
                ac_content = parsed_content;
                //If state is closed or employee/administrator wants to send email notification to user
                if (n_state == 4 || send_email == "1")
                {
                    if (n_state == 4)
                    {
                        //If issue has been closed, this message is sent
                        //Customer data
                        Issue cu_issue = new Issue();
                        cu_issue = db.Issues.Find(id);
                        //customer info
                        MembershipUser cms = Membership.GetUser(cu_issue.Writer);
                        //employee info
                        MembershipUser ems = Membership.GetUser(c_employee);
                        //mail(from, to)
                        MailMessage e_msg = new MailMessage(ems.Email, cms.Email);
                        //String to issue
                        string issueURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port) + "/Issue/Details/" + id;
                        //Subject
                        e_msg.Subject = "IRM: Issue " + id + " has been archived";
                        //Body
                        e_msg.Body = "Dear customer " + cu_issue.Writer + ".\nYour issue id: " + cu_issue.IssueID + " has been resolved and archived by our employee " + c_employee + ".\nLink to issue " + issueURL + "\n" + c_employee + " message: " + ac_content + ".\nContact: " + ems.Email + ".";

                        //Definitions

                        //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                        smtps.UseDefaultCredentials = false;
                        smtps.Credentials = new NetworkCredential("avanadg1@gmail.com", "3l173h4x");
                        smtps.EnableSsl = true;
                        //Send
                        smtps.Send(e_msg);
                        //client.Send(e_msg);
                    }
                    else
                    {
                        //If issue has been closed, this message is sent
                        //Customer data
                        Issue cu_issue = new Issue();
                        cu_issue = db.Issues.Find(id);
                        //customer info
                        MembershipUser cms = Membership.GetUser(cu_issue.Writer);
                        //employee info
                        MembershipUser ems = Membership.GetUser(c_employee);
                        //mail(from, to)
                        MailMessage e_msg = new MailMessage(ems.Email, cms.Email);
                        //String to issue
                        string issueURL = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port) + "/Issue/Details/" + id;
                        //Subject
                        e_msg.Subject = "IRM: Issue " + id;
                        //Body
                        e_msg.Body = "Link to issue " + issueURL + "\n" + c_employee + " message: " + ac_content + ".\nContact: " + ems.Email + ".";

                        //Definitions

                        //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                        smtps.UseDefaultCredentials = false;
                        smtps.Credentials = new NetworkCredential("avanadg1@gmail.com", "3l173h4x");
                        smtps.EnableSsl = true;
                        //Send
                        smtps.Send(e_msg);
                    }
                }

                var issue_sql = @"UPDATE [Issue] SET State = {0}, Modiefied = {1}, Employee = {2} WHERE IssueID = {3}";
                var activity_sql = @"INSERT INTO [Activity] (IssueID, Added, Employee, Context) VALUES ({0}, {1}, {2}, {3})";
               
                db.Database.ExecuteSqlCommand(issue_sql, n_state, mod, employee, id);
                db.Database.ExecuteSqlCommand(activity_sql, id, mod, c_employee, ac_content);
               
                return RedirectToAction("Index");

          

            
        }

        //
        // GET: /Issue/Delete/5
        [Authorize(Roles = "Administrator")]
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

        //Search issues
        // POST: /Issue/Search
        [HttpPost]
        public ActionResult Search(FormCollection c)
        {
            string query = "";
            string s = c["search"];
            string c_user = System.Web.HttpContext.Current.User.Identity.Name;
            if (System.Web.HttpContext.Current.User.IsInRole("Customer"))
            {
                query = "SELECT * FROM Issue WHERE (IssueID LIKE '%"+s+"%' OR Title LIKE '%"+s+"%' OR Description LIKE '%"+s+"%') AND Writer='" + c_user + "'";
            }
            else
            {
                query = "SELECT * FROM Issue WHERE IssueID LIKE '%" + s + "%' OR Title LIKE '%" + s + "%' OR Description LIKE '%" + s + "%' OR Writer LIKE '%"+s+"%'";
            }
            var viewModel = new IssueListViewModel
            {
                lissue = db.Issues.SqlQuery(query)
            };

            return View(viewModel);
        }


        //HTTPOST GetReport
        [HttpPost]
        public ActionResult GetReport(int[] reportID)
        {
            List<IssueViewModel> issueViewModels = new List<IssueViewModel>();
            int al = reportID.Length;
            for(int i = 0; i<al; i++){
                var view = new IssueViewModel {
                    cissue = db.Issues.Find(reportID[i]),
                    cactivities = db.Activities.SqlQuery("SELECT * FROM Activity WHERE IssueID='"+reportID[i]+"'")
                };
                issueViewModels.Add(view);
            }
            
            var viewModel = new ReportIssueViewModel
            {
                report_issues = issueViewModels
            };
           

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}