using System.Web;

namespace uvrp.Models
{
    public class TemplateViewModel
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string Section { get; set; }
        public bool IsImage { get; set; }
        public string Content { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string ImageName { get; set; }
    }
}