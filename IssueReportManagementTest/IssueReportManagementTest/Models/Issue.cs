using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IssueReportManagementTest.Models
{
    public class Issue
    {
        [Key]
        public int IssueID { get; set; }
        public DateTime Added { get; set; }
        public DateTime Modiefied { get; set; }

        public int State { get; set; }


        public virtual Category Category { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public virtual Priority Priority { get; set; }
        public IEnumerable<Category> Priorities { get; set; }

        [ForeignKey("Priority")]
        public int PriorityID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Writer { get; set; }

    }
}