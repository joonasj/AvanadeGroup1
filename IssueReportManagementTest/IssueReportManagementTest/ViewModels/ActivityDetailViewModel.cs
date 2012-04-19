using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueReportManagementTest.ViewModels
{
    public class ActivityDetailViewModel
    {
        public List<string> IssueActivities { get; set; }
        public string IssueTitle { get; set; }
        public string IssueContent { get; set; }
        public string IssueCategory { get; set; }
        public string IssuePriority { get; set; }

    }
}