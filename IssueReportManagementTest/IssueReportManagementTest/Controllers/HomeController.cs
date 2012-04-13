using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueReportManagementTest.ViewModel;
using IssueReportManagementTest.Models;

namespace IssueReportManagementTest.Controllers
{
    public class HomeController : Controller
    {
        private IssueContext db = new IssueContext();

        public ActionResult Index(string mode)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                string query;
                ViewBag.Message = "Welcome to ASP.NET MVC!";
                if (System.Web.HttpContext.Current.User.IsInRole("Employee"))
                {
                    string cuser = System.Web.HttpContext.Current.User.Identity.Name;

                    if (mode != null)
                    {
                        switch (mode)
                        {
                            case "state":
                                query = "SELECT * FROM Issues WHERE Writer='" + cuser + "' ORDER BY State ASC";
                                break;
                            case "cat":
                                query = "SELECT * FROM Issues WHERE Writer='" + cuser + "' ORDER BY CategoryID ASC";
                                break;
                            case "priority":
                                query = "SELECT * FROM Issues WHERE Writer='" + cuser + "' ORDER BY PriorityID ASC";
                                break;
                            case "title":
                                query = "SELECT * FROM Issues WHERE Writer='" + cuser + "' ORDER BY Title ASC";
                                break;
                            default:
                                query = "SELECT * FROM Issues WHERE Writer='" + cuser + "' ORDER BY State ASC";
                                break;
                        }
                    }
                    else
                    {
                        query = "SELECT * FROM Issues WHERE Writer='" + cuser + "' ORDER BY State ASC";
                    }
                    var listIssueviewModel = new IssueListViewModel
                    {
                        lissue = db.Issues.SqlQuery(query)
                    };
                    return View(listIssueviewModel);
                }
                else
                {
                    if (mode != null)
                    {
                        switch (mode)
                        {
                            case "state":
                                query = "SELECT * FROM Issues ORDER BY State ASC";
                                break;
                            case "cat":
                                query = "SELECT * FROM Issues ORDER BY CategoryID ASC";
                                break;
                            case "priority":
                                query = "SELECT * FROM Issues ORDER BY PriorityID ASC";
                                break;
                            case "title":
                                query = "SELECT * FROM Issues ORDER BY Title ASC";
                                break;
                            default:
                                query = "SELECT * FROM Issues ORDER BY State ASC";
                                break;
                        }
                    }
                    else
                    {
                        query = "SELECT * FROM Issue ORDER BY State ASC";
                    }
                    var listIssueviewModel = new IssueListViewModel
                    {
                        lissue = db.Issues.SqlQuery(query)
                    };
                    return View(listIssueviewModel);

                }
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
