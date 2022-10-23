using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using Microsoft.Ajax.Utilities;

namespace uvrp.Controllers
{
    [AllowAnonymous]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RequestEvent()
        {
            return View();
        }

        public ActionResult KilleenCommunity()
        {
            ViewBag.Message = "Your Killeen Community page.";

            return View();
        }
        public ActionResult Partners()
        {
            ViewBag.Message = "Your Partners page.";

            return View();
        }

        [HttpGet]
        public FileResult ExportToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Id"),
                                                     new DataColumn("OrganizationName"),
                                                     new DataColumn("Industry"),
                                                     new DataColumn("PointOfContact"),
                                                     new DataColumn("PhoneNumber")});

            List<Directory> direct;
            using (var ctx = new UVRPEntities1()) {
                direct = ctx.Directory.ToList();
            }
                foreach (var d in direct)
                {
                    dt.Rows.Add(d.Id, d.OrganizationName, d.Industry, d.PointOfContact, d.PhoneNumber);
                }

            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                }
            }
        }


        public ActionResult Directory(string sortOrder, string searchString)
        {
            ViewBag.Message = "Your Directory page.";
            List<Directory> workf;
            using (var ctx = new UVRPEntities1() ) {
                workf = ctx.Directory.ToList();
            }


            if (!String.IsNullOrEmpty(searchString)) {
                workf = workf.Where(x => x.OrganizationName.Contains(searchString)).ToList();
            }

            ViewBag.OrganizationSortParm = String.IsNullOrEmpty(sortOrder) ? "org_desc" : "";
            ViewBag.IndustrySortParm = sortOrder == "Industry" ? "ind_desc" : "Industry";

            switch (sortOrder)
            {
                case "org_desc":
                    workf = workf.OrderByDescending(s => s.OrganizationName ).ToList();
                    break;
                case "Industry":
                    workf = workf.OrderBy(s => s.Industry).ToList();
                    break;
                case "ind_desc":
                    workf = workf.OrderByDescending(s => s.Industry).ToList();
                    break;
                default:
                    workf = workf.OrderBy(s => s.OrganizationName).ToList();
                    break;
            }

            return View(workf);
        }

        public ActionResult Resources()
        {
            ViewBag.Message = "Your Resources page.";

            return View();
        }

        public ActionResult Workforce(string sortOrder)
        {
            List<WorkForce> workf;
            ViewBag.Message = "Your Workforce page.";
            ViewBag.PrioritySortParm = String.IsNullOrEmpty(sortOrder) ? "priority_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";

            using (var ctx = new UVRPEntities1() )
            {
                workf = ctx.WorkForce.ToList();
            }

            switch (sortOrder) {
                case "priority_desc":
                    workf = workf.OrderByDescending(s => s.PriorityNumber).ToList();
                    break;
                case "Date":
                    workf = workf.OrderBy(s => s.DateCreated).ToList();
                    break;
                case "date_desc":
                    workf = workf.OrderByDescending(s => s.DateCreated).ToList();
                    break;
                case "Title":
                    workf = workf.OrderBy(s => s.Title).ToList();
                    break;
                case "title_desc":
                    workf = workf.OrderByDescending(s => s.Title).ToList();
                    break;
                default:
                    workf = workf.OrderBy(s => s.PriorityNumber).ToList();
                    break;
            }

            return View(workf);
        }

        public ActionResult Training()
        {
            ViewBag.Message = "Your Training page.";

            return View();
        }
        public ActionResult UniversityResearch()
        {
            ViewBag.Message = "Your University & Research page.";

            return View();
        }
        public ActionResult DataReports()
        {
            ViewBag.Message = "Your Data & Reports page.";

            return View();
        }
        public ActionResult Videos()
        {
            ViewBag.Message = "Your Videos page.";

            using (var ctx = new UVRPEntities1())
            {
                return View(ctx.Video.ToList());
            }

                return View();
        }
        public ActionResult EventsNews()
        {
            ViewBag.Message = "Your Events and News page.";

            return View();
        }
        public ActionResult ContactUs()
        {
            ViewBag.Message = "Your Contact Us page.";

            return View();
        }

        [HttpGet]
        public ActionResult GetPartnersList()
        {
            List<Partners> partners;
            using (var context = new UVRPEntities1())
            {
                partners = context.Partners.ToList();
            }

            return Json(partners, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetHomePageBanners()
        {
            List<Banner> banners;
            using (var context = new UVRPEntities1())
            {
                banners = context.Banner.Where(x=> x.IsActive == true).ToList();
            }

            return Json(banners, JsonRequestBehavior.AllowGet);

        }
        
        public ActionResult Download(int id)
        {
            string fileName;
            using (var ctx = new UVRPEntities1() ) {
                fileName = ctx.WorkForce.Where(x => x.Id == id).FirstOrDefault().DownloadFileName;
            }

            return File($"~/Content/files/{fileName}", System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public ActionResult SaveMessage(Contacts message)
        {
            Contacts response;
            using (var context = new UVRPEntities1())
            {
                response =  context.Contacts.Add(message);
                context.SaveChanges();    
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult GetAllPublishedPosts()
        {
            List<Posts> PublishedPosts;
            using (var ctx = new UVRPEntities1())
            {
                PublishedPosts = ctx.Posts.Where(p => p.PostStatus == "Published").OrderByDescending(p => p.Id).ToList();
            }

            return Json(PublishedPosts, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllDirectoryListings()
        {
            List<Directory> directories;
            using (var ctx = new UVRPEntities1())
            {
                directories = ctx.Directory.ToList();
            }

            return Json(directories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewPost(int id)
        {
            Posts selected;
            using (var ctx = new UVRPEntities1()) {
                selected = ctx.Posts.Where(p => p.Id == id).FirstOrDefault();
            }

            return View(selected);
        }

        [HttpGet]
        public ActionResult GetPostContentById(int id)
        {
            string PostContent; 

            using (var ctx = new UVRPEntities1())
            {
                PostContent = ctx.Posts.Where(p => p.Id == id).FirstOrDefault().Content;
            }

            return Json(PostContent, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetFooter()
        {
            using (var ctx = new UVRPEntities1())
            {
                return Json(ctx.Footer.FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetHomePageContent()
        {
            using (var ctx = new UVRPEntities1())
            {
                return Json(ctx.HomePageContent.FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }            
        }

        [HttpGet]
        public ActionResult GetInnerPageContent(string id)
        {
            using (var ctx = new UVRPEntities1())
            {
                return Json(ctx.Template.Where(x => x.PageName == id).ToList(), JsonRequestBehavior.AllowGet);
            } 
        }

        [HttpGet]
        public ActionResult GetResources()
        {
            using (var ctx = new UVRPEntities1())
            {
                return Json(ctx.Resources.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetTeams()
        {
            using (var ctx = new UVRPEntities1())
            {
                return Json(ctx.Team.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        
        
        [AllowAnonymous]
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return RedirectToAction("Error");
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            Response.StatusCode = 503;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
        
    }
}