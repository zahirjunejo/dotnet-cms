using System.Web;

namespace uvrp.Models
{
    public class HomePageBannerModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public HttpPostedFileBase Upload { get; set; }
        
        public string ImageData { get; set; }
        public string UploadFileName { get; set; }
        public string AltText { get; set; }
        public string Abstract { get; set; }
        public bool IsActive { get; set; }   
    }
}