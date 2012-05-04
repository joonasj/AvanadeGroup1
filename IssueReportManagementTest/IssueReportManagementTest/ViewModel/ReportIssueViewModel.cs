using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IssueReportManagementTest.Models;
using IssueReportManagementTest.ViewModel;
namespace IssueReportManagementTest.ViewModel
{
    public class ReportIssueViewModel
    {
        public IEnumerable<IssueViewModel> report_issues {get; set;}

    }
}