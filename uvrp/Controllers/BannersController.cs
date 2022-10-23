using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uvrp;
using uvrp.Models;

namespace uvrp.Controllers
{
    public class BannersController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: Banners
        public async Task<ActionResult> Index()
        {
            return View(await db.Banner.ToListAsync());
        }

        public async Task<ActionResult> Main()
        {
            return View();
        }

        // GET: Banners/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = await db.Banner.FindAsync(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // GET: Banners/Create
        public ActionResult Create()
        {
            HomePageBannerModel hpb = new HomePageBannerModel();
            hpb.IsActive = true;
            return View(hpb);
        }

        // POST: Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(HomePageBannerModel hpb)
        {
            
            
            Banner banner = new Banner();

            using (var ctx = new UVRPEntities1())
            {
                banner.Title = hpb.Title;
                banner.Abstract = hpb.Abstract;
                banner.AltText = hpb.AltText;
                banner.IsActive = hpb.IsActive;
                banner.DateAdded = DateTime.Now.ToString();

                try
                {
                    if (hpb.Upload != null && hpb.Upload.ContentLength > 0)
                    {
                        string _FileName = hpb.Upload.FileName;
                        
                        if (hpb.ImageData.Length > 0)
                        {
                            byte[] bytes = Convert.FromBase64String(hpb.ImageData.Split(',')[1]);
                            string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                            if (System.IO.File.Exists(_path))
                                Trace.WriteLine("File already exists.");
                            else
                            {
                                using (FileStream stream = new FileStream(Server.MapPath($"~/Content/img/{_FileName}"), FileMode.Create))
                                {
                                    stream.Write(bytes, 0, bytes.Length);
                                    stream.Flush();
                                }
                            }

                        }
                        else
                        {
                            string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                            if (System.IO.File.Exists(_path))
                                Trace.WriteLine("File aready exists.");
                            else
                                hpb.Upload.SaveAs(_path);                              
                        }
                        

                        banner.Upload = _FileName;
                    }
                }
                catch
                {
                    ViewBag.UploadMessage = "Upload error";
                    return View();
                }

                ctx.Banner.Add(banner);
                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Banners/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = await db.Banner.FindAsync(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            
            HomePageBannerModel hpb = new HomePageBannerModel();
            hpb.Id = banner.Id;
            hpb.Title = banner.Title;
            hpb.Abstract = banner.Abstract;
            hpb.AltText = banner.AltText;
            hpb.IsActive = banner.IsActive == true ? true : false;
            hpb.UploadFileName = banner.Upload;

            return View(hpb);
        }

        // POST: Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(HomePageBannerModel homePageBanner)
        {
            
            Console.WriteLine($"base 64 {homePageBanner.ImageData}");
            using (var ctx = new UVRPEntities1())
            {
                Banner banner = ctx.Banner.Find(homePageBanner.Id);
                banner.Title = homePageBanner.Title;
                banner.Abstract = homePageBanner.Abstract;
                banner.AltText = homePageBanner.AltText;
                banner.IsActive = homePageBanner.IsActive;
                try
                {
                    if (homePageBanner.Upload != null && homePageBanner.Upload.ContentLength > 0)
                    {
                        string _FileName = homePageBanner.Upload.FileName;
                        
                        if (homePageBanner.ImageData.Length > 0)
                        {
                            byte[] bytes = Convert.FromBase64String(homePageBanner.ImageData.Split(',')[1]);
                            string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                            if (System.IO.File.Exists(_path))
                                Console.WriteLine("File already exists.");
                            else
                            {
                                using (FileStream stream = new FileStream(Server.MapPath($"~/Content/img/{_FileName}"), FileMode.Create))
                                {
                                    stream.Write(bytes, 0, bytes.Length);
                                    stream.Flush();
                                }
                            }

                        }
                        else
                        {
                            string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                            if (System.IO.File.Exists(_path))
                                Trace.WriteLine("File aready exists.");
                            else
                                homePageBanner.Upload.SaveAs(_path);                               
                        }

                        banner.Upload = _FileName;
                    }
                }
                catch
                {
                    ViewBag.UploadMessage = "Upload error";
                    return View();
                }
                
                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Banners/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            Banner banner = await db.Banner.FindAsync(id);
            db.Banner.Remove(banner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Banner banner = await db.Banner.FindAsync(id);
            db.Banner.Remove(banner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
