using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IssueReportManagementTest.Models
{
    public class Activity
    {
        [Key]
        public int ActivityID { get; set; }
        //Added automatic
        public DateTime Added { get; set; }
        //Added automatic
        public string Employee { get; set; }
        //What has employee done
        public string Context { get; set; }
        //What issue is this activity using
        
        public virtual Issue Issue { get; set; }
        public IEnumerable<Issue> Issues { get; set; }
        [ForeignKey("Issue")]
        public int IssueID { get; set; }
        

    }
}