using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _8DManagementSystem.Models
{
    public class Report_WorkFlow_Log_Model
    {

        public Guid ReportGuid { get; set; }

        public string Comment { get; set; }

        public int ReportStatus { get; set; }

        public int ReportCancelStatus { get; set; }
    }
}