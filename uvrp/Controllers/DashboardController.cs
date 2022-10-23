using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace uvrp.Controllers
{

    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        
        
        public ActionResult Settings()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetDashboardMetrics()
        {

            DashboardMetricsModel DMModel = new DashboardMetricsModel();
            
            using (var ctx = new UVRPEntities1()) {
                DMModel.Count = ctx.Posts.Count();
                DMModel.LatestPost = ctx.Posts.Where(p => p.PostStatus == "Published").OrderByDescending(p => p.Id).FirstOrDefault();
                DMModel.PublishedCount = ctx.Posts.Where(p => p.PostStatus == "Published").Count();
                DMModel.TrashCount = ctx.Posts.Where(p => p.PostStatus == "Deleted").Count();
                DMModel.PendingCount = ctx.Posts.Where(p => p.PostStatus == "Pending").Count();
            }

            return Json(DMModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Settings(HttpPostedFileBase logo){
            try
            {
                
                if (logo.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(logo.FileName);
                    string extension = Path.GetExtension(logo.FileName);

                    Trace.WriteLine($"company_logo{extension}");

                    string _path = Path.Combine(Server.MapPath("~/Content/img"), $"company_logo{extension}");
                    logo.SaveAs(_path);
                }

                ViewBag.Message = "Logo updated successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "Logo update failed!!";
                return View();
            }
        }

    }

    internal class DashboardMetricsModel
    {
        public int Count = 0;
        public Posts LatestPost;
        public int PublishedCount;
        public int TrashCount;
        public int PendingCount;
    }
}