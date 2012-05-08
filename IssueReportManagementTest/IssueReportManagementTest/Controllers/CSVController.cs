using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using IssueReportManagementTest.Models;
using IssueReportManagementTest.ViewModel;

namespace IssueReportManagementTest.Controllers
{
    public class CSVController : Controller
    {
        private IssueContext db = new IssueContext();
        private string csvdata;
        //
        // GET: /CSV/
        [HttpPost]
        public ActionResult Download(int[] reportID)
        {
            csvdata = "";
            List<IssueViewModel> issueViewModels = new List<IssueViewModel>();
            int al = reportID.Length;
            for (int i = 0; i < al; i++)
            {
                var view = new IssueViewModel
                {
                    cissue = db.Issues.Find(reportID[i]),
                    cactivities = db.Activities.SqlQuery("SELECT * FROM Activity WHERE IssueID='" + reportID[i] + "'")
                };
                issueViewModels.Add(view);
                
            }
            foreach (IssueViewModel item in issueViewModels)
            {
                csvdata = csvdata + item.cissue.Title + "," + item.cissue.Added + "," + item.cissue.Modiefied + "," + item.cissue.State + "," + item.cactivities.Count() + ",";
            }
            //string csvdata;

            /*var viewModel = new ReportIssueViewModel
            {
                report_issues = issueViewModels
            };*/

            //string billcsv = "account_ref,line1,line2,line3";
            var data = Encoding.UTF8.GetBytes(csvdata);
            string filename = "reports"+DateTime.Today.ToString("dd-MM-yy")+".csv";
            return File(data, "text/csv", filename);
        }


    }
}
