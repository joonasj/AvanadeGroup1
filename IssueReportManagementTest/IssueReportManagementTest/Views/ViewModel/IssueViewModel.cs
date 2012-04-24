using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IssueReportManagementTest.Models;

namespace IssueReportManagementTest.ViewModel
{
    public class IssueViewModel
    {

        public Issue cissue { get; set; }
        public IEnumerable<Activity> cactivities { get; set; }
        public string cstate { get; set; }
    }
}