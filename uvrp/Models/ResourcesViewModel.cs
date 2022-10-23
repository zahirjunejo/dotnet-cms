using System.ComponentModel;
using System.Web;

namespace uvrp.Models
{
    public class ResourcesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase IconImage { get; set; }
    }
}