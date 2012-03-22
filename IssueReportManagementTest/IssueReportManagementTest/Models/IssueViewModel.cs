using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueReportManagementTest.Models
{
    public class IssueViewModel
    {
        public int SelectedCategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

    }
}