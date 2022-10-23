using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uvrp;
using uvrp.Models;
using System.IO;
using System.Diagnostics;
using WebGrease;

namespace uvrp.Controllers
{
    public class DirectoriesController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: Directories
        public async Task<ActionResult> Index()
        {
            return View(await db.Directory.ToListAsync());
        }

        // GET: Directories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directory directory = await db.Directory.FindAsync(id);
            if (directory == null)
            {
                return HttpNotFound();
            }
            return View(directory);
        }

        // GET: Directories/Create
        public ActionResult Create()
        {
            List<SelectListItem> darksied = new List<SelectListItem>();
            using (var ctx = new UVRPEntities1())
            {
                foreach (var item in ctx.Masters.Where(x => x.Type == "Industry").ToList())
                {
                    darksied.Add(new SelectListItem { Text=item.Text, Value = item.Value, Selected = false} );
                }
                ViewBag.IndustryList = new SelectList(darksied, "Value", "Text", ctx.Masters.First(x => x.Type == "Industry").Value);
            }

            return View();
        }

        // POST: Directories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DirectoriesAddViewModel davm)
        {
            if (ModelState.IsValid)
            {
                Directory directory = new Directory();
                directory.OrganizationName = davm.OrganizationName;
                directory.Industry = davm.Industry;

                try
                {
                    if (davm.Logo !=null && davm.Logo.ContentLength > 0)
                    {
                        string _FileName = davm.Logo.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Trace.WriteLine("File aready exists.");
                        else
                            davm.Logo.SaveAs(_path);
                        
                        directory.Logo = _FileName;
                    }

                    ViewBag.UploadMessage = "Logo uploaded successfully";

                    directory.Website = davm.Website;
                    directory.ContactAddress = davm.ContactAddress;
                    directory.PhoneNumber = davm.PhoneNumber;
                    directory.PointOfContact = davm.PointOfContact;
                    directory.EmailAddress = davm.EmailAddress;
                    directory.IsAddressPrivate = davm.IsAddressPrivate;
                    directory.IsEmailPrivate = davm.IsEmailPrivate;
                    directory.IsPhonePrivate = davm.IsPhonePrivate;

                    db.Directory.Add(directory);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");

                }
                catch
                {
                    ViewBag.UploadMessage = "Logo upload error.";
                    return View();
                }

            }
            
            ViewBag.IndustryList = new SelectList(
                        new List<SelectListItem> {
                                new SelectListItem { Text="Defense", Value="Defense", Selected=true },
                                new SelectListItem { Text="Education", Value="Education", Selected=false },
                                new SelectListItem { Text="Government", Value="Government", Selected=false }
                        }, "Value", "Text", "Defense");

            return View(davm);

        }

        // GET: Directories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directory directory = await db.Directory.FindAsync(id);
            if (directory == null)
            {
                return HttpNotFound();
            }

            using (var ctx = new UVRPEntities1())
            {
                List<SelectListItem> darksied = new List<SelectListItem>();
                foreach (var item in ctx.Masters.Where(x => x.Type == "Industry").ToList())
                {
                    darksied.Add(new SelectListItem { Text=item.Text, Value = item.Value, Selected = false} );
                }
                ViewBag.IndustryList = new SelectList(darksied, "Value", "Text", directory.Industry);
            }

            return View(directory);
        }

        // POST: Directories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DirectoriesAddViewModel davm)
        {
            using (var ctx = new UVRPEntities1()) {
                Directory foundDir = ctx.Directory.Find(davm.Id);
                foundDir.Industry = davm.Industry;
                foundDir.OrganizationName = davm.OrganizationName;

                try
                {
                    if (davm.Logo != null && davm.Logo.ContentLength > 0)
                    {
                        string _FileName = davm.Logo.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Trace.WriteLine("File aready exists.");
                        else
                            davm.Logo.SaveAs(_path);

                        foundDir.Logo = _FileName;
                    }

                    ViewBag.UploadMessage = "Logo uploaded successfully";
                }
                catch
                {
                    ViewBag.UploadMessage = "Logo upload error.";
                    return View();
                }

                foundDir.Website = davm.Website;
                foundDir.ContactAddress = davm.ContactAddress;
                foundDir.PhoneNumber = davm.PhoneNumber;
                foundDir.PointOfContact = davm.PointOfContact;
                foundDir.EmailAddress = davm.EmailAddress;
                foundDir.IsEmailPrivate = davm.IsEmailPrivate;
                foundDir.IsAddressPrivate = davm.IsAddressPrivate;
                foundDir.IsPhonePrivate = davm.IsPhonePrivate;

                ctx.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        // GET: Directories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            Directory directory = await db.Directory.FindAsync(id);
            db.Directory.Remove(directory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: Directories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Directory directory = await db.Directory.FindAsync(id);
            db.Directory.Remove(directory);
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
