using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IssueReportManagementTest.Models;

namespace IssueReportManagementTest.ViewModel
{
    public class IssueListViewModel
    {
        public IEnumerable<Issue> lissue { get; set; }
        public IEnumerable<Issue> a_lissue { get; set; }
        public IEnumerable<Issue> e_lissue { get; set; }
    }
}