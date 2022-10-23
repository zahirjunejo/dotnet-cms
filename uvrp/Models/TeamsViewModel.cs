using System.Web;

namespace uvrp.Models
{
    public class TeamsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}