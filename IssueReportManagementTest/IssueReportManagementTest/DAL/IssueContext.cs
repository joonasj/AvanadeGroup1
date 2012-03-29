using System;
using System.Collections.Generic;
using System.Data.Entity;
using IssueReportManagementTest.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace IssueReportManagementTest.Models
{
    public class IssueContext : DbContext
    {
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}