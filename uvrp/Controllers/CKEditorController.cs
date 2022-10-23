using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using uvrp.Models;

namespace uvrp.Controllers
{
    
    [AllowAnonymous]
    public class CKEditorController : Controller
    {

        public ActionResult uploadnow(HttpPostedFileWrapper upload)
        {
            
                string ImageName = upload.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/CKImages"), ImageName);
                upload.SaveAs(path);
            

            return Json(new
            {
                fileName=ImageName,
                uploaded=true,
                url="/Content/CKImages/"+upload.FileName
            });
        }
        
        public ActionResult uploadPartial()
        {
            var appData = Server.MapPath("~/Content/CKImages");
            var images = System.IO.Directory.GetFiles(appData).Select(x => new ImagesViewModel
            {
                Url = Url.Content("/Content/CKImages/" + Path.GetFileName(x))
            });
            
            return View(images);
        }

    }
}