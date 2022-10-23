using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uvrp.Models
{
    public class WorkForceViewModel
    {

        public int Id { get; set; }
        public int PriorityNumber { get; set; }
        public string Title { get; set; }
        public HttpPostedFileBase DownloadFile { get; set; }
        public string FileName { get; set; }
    }
}