using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class TemplatesController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: Templates
        public async Task<ActionResult> Index(string id)
        {
            if (id != null)
            {
                return View(await db.Template.Where(x => x.PageName == id).ToListAsync());
            }
            else
            {
                return View(await db.Template.ToListAsync());
            }

        }
        
        public async Task<ActionResult> Main()
        {
            return View();
        }

        // GET: Templates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = await db.Template.FindAsync(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // GET: Templates/Create
        public ActionResult Create(string id)
        {
            List<SelectListItem> xList = new List<SelectListItem>();
            xList.Add(new SelectListItem {Text="Section1", Value="Section1", Selected= false } );

            if (id != "Error")
            {
                xList.Add(new SelectListItem {Text="Banner", Value="Banner", Selected= false } );
                xList.Add(new SelectListItem {Text="Section2", Value="Section2", Selected= false } );
                xList.Add(new SelectListItem {Text="Section3", Value="Section3", Selected= false } );
            }

            ViewBag.xList = new SelectList(xList, "Value", "Text", "Section1");
            
            if (id != null)
            {
                TemplateViewModel templateViewModel = new TemplateViewModel();
                templateViewModel.PageName = id;
                
                return View(templateViewModel);
            }

            return View();
        }

        // POST: Templates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(TemplateViewModel tvm)
        {
             Template template = new Template();

            using (var ctx = new UVRPEntities1())
            {
                template.PageName = tvm.PageName;
                template.Section = tvm.Section;
                template.Content = tvm.Content;
                
                try
                {
                    if (tvm.Image != null && tvm.Image.ContentLength > 0)
                    {
                        string _FileName = tvm.Image.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Console.WriteLine("File aready exists.");
                        else
                            tvm.Image.SaveAs(_path);

                        template.Image = _FileName;
                    }
                }
                catch
                {
                    ViewBag.UploadMessage = "Upload error";
                    return View();
                }                                        
                

                ctx.Template.Add(template);
                ctx.SaveChanges();
            }

            return RedirectToAction("Main");
        }

        // GET: Templates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            List<SelectListItem> xList = new List<SelectListItem>();
//            xList.Add(new SelectListItem {Text="Banner", Value="Banner", Selected= false } );
//            xList.Add(new SelectListItem {Text="Section1", Value="Section1", Selected= false } );
//            xList.Add(new SelectListItem {Text="Section2", Value="Section2", Selected= false } );
//            xList.Add(new SelectListItem {Text="Section3", Value="Section3", Selected= false } );

            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Template template = await db.Template.FindAsync(id);

            if (template == null)
            {
                return HttpNotFound();
            }
            
            TemplateViewModel tvm = new TemplateViewModel();
            tvm.Id = template.Id;
            tvm.PageName = template.PageName;
            tvm.Content = template.Content;
            tvm.Section = template.Section;
            tvm.ImageName = template.Image;

            xList.Add(new SelectListItem {Text="Section1", Value="Section1", Selected= false } );

            if (template.PageName != "Error")
            {
                xList.Add(new SelectListItem {Text="Banner", Value="Banner", Selected= false } );
                xList.Add(new SelectListItem {Text="Section2", Value="Section2", Selected= false } );
                xList.Add(new SelectListItem {Text="Section3", Value="Section3", Selected= false } );
            }
            
            ViewBag.xList = new SelectList(xList, "Value", "Text", template.Section);
            
            return View(tvm);
        }
        
        public async Task<ActionResult> GoEdit(string pageName, string section)
        {
            List<SelectListItem> xList = new List<SelectListItem>();

            if (pageName == null || section == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
//            Template template = await db.Template.FindAsync(id);
            Template template;
            using (var ctx = new UVRPEntities1())
            {
                template = ctx.Template.FirstOrDefault(x => x.PageName == pageName && x.Section == section);
            }
            
            if (template == null)
            {
//                throw new HttpException(404, "Not found");
                TemplateViewModel createtvm = new TemplateViewModel();
                createtvm.PageName = pageName;
                createtvm.Section = section;

                xList.Add(new SelectListItem {Text="Section1", Value="Section1", Selected= false } );

                if (pageName != "Error")
                {
                    xList.Add(new SelectListItem {Text="Banner", Value="Banner", Selected= false } );
                    xList.Add(new SelectListItem {Text="Section2", Value="Section2", Selected= false } );
                    xList.Add(new SelectListItem {Text="Section3", Value="Section3", Selected= false } );
                }
            
                ViewBag.xList = new SelectList(xList, "Value", "Text", section);
            
                return View("create", createtvm);
            }
            
            TemplateViewModel tvm = new TemplateViewModel();
            tvm.Id = template.Id;
            tvm.PageName = template.PageName;
            tvm.Content = template.Content;
            tvm.Section = template.Section;
            tvm.ImageName = template.Image;

            xList.Add(new SelectListItem {Text="Section1", Value="Section1", Selected= false } );

            if (template.PageName != "Error")
            {
                xList.Add(new SelectListItem {Text="Banner", Value="Banner", Selected= false } );
                xList.Add(new SelectListItem {Text="Section2", Value="Section2", Selected= false } );
                xList.Add(new SelectListItem {Text="Section3", Value="Section3", Selected= false } );
            }
            
            ViewBag.xList = new SelectList(xList, "Value", "Text", template.Section);
            
            return View("Edit", tvm);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(TemplateViewModel tvm)
        {
             using (var ctx = new UVRPEntities1())
             {
                Template template = ctx.Template.Find(tvm.Id);
                template.PageName = tvm.PageName;
                template.Section = tvm.Section;
                template.Content = tvm.Content;
                
                try
                {
                    if (tvm.Image != null && tvm.Image.ContentLength > 0)
                    {
                        string _FileName = tvm.Image.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Console.WriteLine("File already exists.");
                        else
                            tvm.Image.SaveAs(_path);

                        template.Image = _FileName;
                    }
                }
                catch
                {
                    ViewBag.UploadMessage = "Upload error";
                    return View();
                }                                        
                

                ctx.SaveChanges();
            }

            return RedirectToAction("Main");
        }

        // GET: Templates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Template template = await db.Template.FindAsync(id);
//            if (template == null)
//            {
//                return HttpNotFound();
//            }
//            return View(template);
            Template template = await db.Template.FindAsync(id);
            db.Template.Remove(template);
            await db.SaveChangesAsync();
            return RedirectToAction("Main");
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Template template = await db.Template.FindAsync(id);
            db.Template.Remove(template);
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
