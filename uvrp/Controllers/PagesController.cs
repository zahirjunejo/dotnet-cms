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
using Microsoft.Ajax.Utilities;
using uvrp;
using uvrp.Models;

namespace uvrp.Controllers
{
    public class PagesController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: Pages
        public async Task<ActionResult> Index(int? id)
        {
            return id != null ? View(await db.Page.Where(x => x.ParentId == id).ToListAsync()) : View(await db.Page.Where(x => x.IsParent == true && x.ParentId == null).ToListAsync());
        }

        // GET: Pages/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Page page;

            using (var ctx = new UVRPEntities1())
            {
                page = ctx.Page.FirstOrDefault(x => x.Slug == id);
            }

            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllPages()
        {
            List<Page> pages = new List<Page>();

            using (var ctx = new UVRPEntities1())
            {
                pages = ctx.Page.Where(x => x.IsActive == true).ToList() ;
            }

            return Json(pages, JsonRequestBehavior.AllowGet);
        }
        
        // GET: Pages/Create
        public ActionResult Create(int? id)
        {
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            List<SelectListItem> xList = new List<SelectListItem>();
            xList.Add(new SelectListItem {Text="", Value="", Selected= false } );
            xList.Add(new SelectListItem {Text="Home", Value=$"{baseUrl}/Home/Index", Selected= false } );
            xList.Add(new SelectListItem {Text="About", Value=$"{baseUrl}/Home/About", Selected= false } );
            xList.Add(new SelectListItem {Text="Killeen Community", Value=$"{baseUrl}/Home/KilleenCommunity", Selected= false } );
            xList.Add(new SelectListItem {Text="Workforce", Value=$"{baseUrl}/Home/Workforce", Selected= false } );
            xList.Add(new SelectListItem {Text="Resources", Value=$"{baseUrl}/Home/Resources", Selected= false } );
            xList.Add(new SelectListItem {Text="Directory", Value=$"{baseUrl}/Home/Directory", Selected= false } );
            xList.Add(new SelectListItem {Text="Contact Us", Value=$"{baseUrl}/Home/ContactUs", Selected= false } );
            xList.Add(new SelectListItem {Text="Data and Reports", Value=$"{baseUrl}/Home/DataReports", Selected= false } );
            xList.Add(new SelectListItem {Text="Partners", Value=$"{baseUrl}/Home/Partners", Selected= false } );
            xList.Add(new SelectListItem {Text="Videos", Value=$"{baseUrl}/Home/Videos", Selected= false } );
            xList.Add(new SelectListItem {Text="Events and News", Value=$"{baseUrl}/Home/EventsNews", Selected= false } );
            xList.Add(new SelectListItem {Text="University and Research", Value=$"{baseUrl}/Home/UniversityResearch", Selected= false } );
            xList.Add(new SelectListItem {Text="Training", Value=$"{baseUrl}/Home/Training", Selected= false } );
            
            ViewBag.xList = new SelectList(xList, "Value", "Text", "");
            
            if (id != null)
            {
                PageViewModel pvm = new PageViewModel();
                pvm.ParentId = id;
                return View(pvm);
            }

            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(PageViewModel pageVM)
        {
            Page page = new Page();

            using (var ctx = new UVRPEntities1())
            {
                page.Title = pageVM.Title;
                page.Subtitle = pageVM.Subtitle;
                page.Content = pageVM.Content;
                page.ParentId = pageVM.ParentId;
                page.IsActive = pageVM.IsActive;
                page.IsCustom = pageVM.IsCustom;
                page.priority = pageVM.priority;

                if (pageVM.ParentId != null)
                {
                    page.IsParent = false;
                    page.HasChild = false;
                    Page parentPage = ctx.Page.Find(pageVM.ParentId);
                    parentPage.HasChild = true;
                    parentPage.IsParent = true;
                    ctx.SaveChanges();
                }
                else
                {
                    page.IsParent = true;
                }


                try
                {
                    if (pageVM.HeaderImage != null && pageVM.HeaderImage.ContentLength > 0)
                    {
                        string _FileName = pageVM.HeaderImage.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Trace.WriteLine("File aready exists.");
                        else
                            pageVM.HeaderImage.SaveAs(_path);

                        page.HeaderImage = _FileName;
                    }
                }
                catch
                {
                    ViewBag.UploadMessage = "Upload error";
                    return View();
                }

                ctx.Page.Add(page);
                ctx.SaveChanges();
                
                page.Slug = $"{page.Title.Trim().ToLower().Replace(' ', '-')}-{page.Id}";

                if (pageVM.IsCustom == true)
                {
                    page.Slug = pageVM.Slug;
                }

                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Pages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = await db.Page.FindAsync(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            
            PageViewModel pageVM = new PageViewModel();
            pageVM.Id = page.Id;
            pageVM.Title = page.Title;
            pageVM.Subtitle = page.Subtitle;
            pageVM.Content = page.Content;
            pageVM.ParentId = page.ParentId;
            pageVM.HeaderImageName = page.HeaderImage;
            pageVM.IsActive = page.IsActive;
            pageVM.IsCustom = page.IsCustom == true ? true : false;
            pageVM.Slug = page.Slug;
            pageVM.priority = page.priority ?? 0 ;
            
            List<SelectListItem> Pages = new List<SelectListItem>();
            
            using (var ctx = new UVRPEntities1())
            {
                
                Pages.Add(new SelectListItem
                {
                    Text="", 
                    Value=""
                } );
                
                foreach (var item in  ctx.Page.Where(x => x.Id != pageVM.Id && x.ParentId == null).ToList() )
                {
                    Pages.Add(new SelectListItem
                    {
                        Text=item.Title, 
                        Value=item.Id.ToString()
                    } );
                }                
            }

            ViewBag.ParentList = new SelectList(Pages, "Value", "Text", pageVM.ParentId.ToString());
            
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            List<SelectListItem> xList = new List<SelectListItem>();
            xList.Add(new SelectListItem {Text="", Value="", Selected= false } );
            xList.Add(new SelectListItem {Text="Home", Value=$"{baseUrl}/Home/Index", Selected= false } );
            xList.Add(new SelectListItem {Text="About", Value=$"{baseUrl}/Home/About", Selected= false } );
            xList.Add(new SelectListItem {Text="Killeen Community", Value=$"{baseUrl}/Home/KilleenCommunity", Selected= false } );
            xList.Add(new SelectListItem {Text="Workforce", Value=$"{baseUrl}/Home/Workforce", Selected= false } );
            xList.Add(new SelectListItem {Text="Resources", Value=$"{baseUrl}/Home/Resources", Selected= false } );
            xList.Add(new SelectListItem {Text="Directory", Value=$"{baseUrl}/Home/Directory", Selected= false } );
            xList.Add(new SelectListItem {Text="Contact Us", Value=$"{baseUrl}/Home/ContactUs", Selected= false } );
            xList.Add(new SelectListItem {Text="Data and Reports", Value=$"{baseUrl}/Home/DataReports", Selected= false } );
            xList.Add(new SelectListItem {Text="Partners", Value=$"{baseUrl}/Home/Partners", Selected= false } );
            xList.Add(new SelectListItem {Text="Videos", Value=$"{baseUrl}/Home/Videos", Selected= false } );
            xList.Add(new SelectListItem {Text="Events and News", Value=$"{baseUrl}/Home/EventsNews", Selected= false } );
            xList.Add(new SelectListItem {Text="University and Research", Value=$"{baseUrl}/Home/UniversityResearch", Selected= false } );
            xList.Add(new SelectListItem {Text="Training", Value=$"{baseUrl}/Home/Training", Selected= false } );
            
            ViewBag.xList = new SelectList(xList, "Value", "Text", $"{page.Slug}");
            
            return View(pageVM);
            
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(PageViewModel pageVM)
        {
            using (var ctx = new UVRPEntities1())
            {
                Page page = ctx.Page.Find(pageVM.Id);
                page.Title = pageVM.Title;
                page.Subtitle = pageVM.Subtitle;
                page.Content = pageVM.Content;
                page.IsActive = pageVM.IsActive;
                page.IsCustom = pageVM.IsCustom;
                page.priority = pageVM.priority;
                
                if (pageVM.ParentId != page.ParentId)
                {
                    if (pageVM.ParentId != null)
                    {
                        Page parentPage = ctx.Page.Find(pageVM.ParentId);
                        parentPage.HasChild = true;
                        parentPage.IsParent = true;
                        ctx.SaveChanges();
                    }

                    if (page.ParentId != null)
                    {
                        if (!ctx.Page.Any( child => child.ParentId == page.ParentId && child.Id != page.Id ))
                        {
                            Page parentPage = ctx.Page.Find(page.ParentId);
                            parentPage.HasChild = false;
                            parentPage.IsParent = false;
                            ctx.SaveChanges();                                                        
                        }
                    }
                    
                }

                page.ParentId = pageVM.ParentId;
                
                try
                {
                    if (pageVM.HeaderImage != null && pageVM.HeaderImage.ContentLength > 0)
                    {
                        string _FileName = pageVM.HeaderImage.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Trace.WriteLine("File aready exists.");
                        else
                            pageVM.HeaderImage.SaveAs(_path);

                        page.HeaderImage = _FileName;
                    }
                }
                catch
                {
                    ViewBag.UploadMessage = "Upload error";
                    return View();
                }
                
                page.Slug = $"{page.Title.Trim().ToLower().Replace(' ', '-')}-{page.Id}";
                
                if (pageVM.IsCustom == true)
                {
                    page.Slug = pageVM.Slug;
                }
                
                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Pages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Page page = await db.Page.FindAsync(id);
//            if (page == null)
//            {
//                return HttpNotFound();
//            }
//            return View(page);
            Page page = await db.Page.FindAsync(id);
            db.Page.Remove(page);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Page page = await db.Page.FindAsync(id);
            db.Page.Remove(page);
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

        public ActionResult Hierarchy()
        {
            return View();
        }
    }
}
