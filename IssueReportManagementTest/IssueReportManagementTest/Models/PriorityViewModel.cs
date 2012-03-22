using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueReportManagementTest.Models
{
    public class PriorityViewModel
    {
        public int PriorityId { get; set; }
        public IEnumerable<Priority> Priorities { get; set; }
    }
}