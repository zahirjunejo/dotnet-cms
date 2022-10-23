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
    public class ResourcesController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: Resources
        public async Task<ActionResult> Index()
        {
            return View(await db.Resources.ToListAsync());
        }

        // GET: Resources/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resources resources = await db.Resources.FindAsync(id);
            if (resources == null)
            {
                return HttpNotFound();
            }
            return View(resources);
        }

        // GET: Resources/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ResourcesViewModel resourcesViewModel)
        {
            try
            {
                
                if (resourcesViewModel.IconImage.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(resourcesViewModel.IconImage.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                    resourcesViewModel.IconImage.SaveAs(_path);
                }

                using (var ctx = new UVRPEntities1()) {
                    ctx.Resources.Add(new Resources {  
                        Title=resourcesViewModel.Title, 
                        Description=resourcesViewModel.Description, 
                        IconImage = Path.GetFileName(resourcesViewModel.IconImage.FileName) 
                    });
                    ctx.SaveChanges();
                }

                ViewBag.Message = "File Uploaded Successfully!!";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }

        // GET: Resources/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resources resources = await db.Resources.FindAsync(id);
            if (resources == null)
            {
                return HttpNotFound();
            }
            return View(resources);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,IconImage")] Resources resources)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resources).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(resources);
        }

        // GET: Resources/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            Resources resources = await db.Resources.FindAsync(id);
            db.Resources.Remove(resources);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Resources resources = await db.Resources.FindAsync(id);
//            if (resources == null)
//            {
//                return HttpNotFound();
//            }
//            return View(resources);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Resources resources = await db.Resources.FindAsync(id);
            db.Resources.Remove(resources);
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
