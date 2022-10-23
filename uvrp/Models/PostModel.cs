using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace uvrp.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        public string Content { get; set; }
        public string Category { get; set; }

        [DisplayName("Status")]
        public string PostStatus { get; set; }

        [DisplayName("Event start date")]
        public string EventStartDate { get; set; }

        [DisplayName("Event end date")]
        public string EventEndDate { get; set; }
        public string Location { get; set; }
        public bool Recurring { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }
        public string Website { get; set; }
        public string Organization { get; set; }

        [DisplayName("Event contact")]
        public string EventContact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [DisplayName("Start time")]
        public string startTime { get; set; }
        [DisplayName("End time")]
        public string endTime { get; set; }

        [DisplayName("Days of week")]
        public string DaysOfWeek { get; set; }
    }
}