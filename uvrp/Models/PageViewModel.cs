using System;
using System.Web;

namespace uvrp.Models
{
    public class PageViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public string HeaderImageName { get; set; }
        public HttpPostedFileBase HeaderImage { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsCustom { get; set; }
        public int priority { get; set; }

    }
}