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

namespace uvrp.Controllers
{
    public class WorkForcesController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: WorkForces
        public ActionResult Index(string sortOrder)
        {
            List<WorkForce> workf;
            ViewBag.PrioritySortParm = String.IsNullOrEmpty(sortOrder) ? "priority_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            using (var ctx = new UVRPEntities1())
            {
                workf = ctx.WorkForce.ToList();
            }

            switch (sortOrder)
            {
                case "priority_desc":
                    workf = workf.OrderByDescending(s => s.PriorityNumber).ToList();
                    break;
                case "Date":
                    workf = workf.OrderBy(s => s.DateCreated).ToList();
                    break;
                case "date_desc":
                    workf = workf.OrderByDescending(s => s.DateCreated).ToList();
                    break;
                default:
                    workf = workf.OrderBy(s => s.PriorityNumber).ToList();
                    break;
            }

            return View(workf);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetWorkForceStuff() {
            List<WorkForce> wf;
            using (var ctx =new UVRPEntities1())
            {
                wf = ctx.WorkForce.ToList();
            }

            return Json(wf, JsonRequestBehavior.AllowGet);
        }

        // GET: WorkForces/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkForce workForce = await db.WorkForce.FindAsync(id);
            if (workForce == null)
            {
                return HttpNotFound();
            }
            return View(workForce);
        }

        // GET: WorkForces/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkForces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkForceViewModel workForceVM)
        {
            WorkForce wf = new WorkForce();
            wf.Title = workForceVM.Title;
            wf.PriorityNumber = workForceVM.PriorityNumber;
            wf.DateCreated = DateTime.Now;

            try
            {
                if (workForceVM.DownloadFile != null && workForceVM.DownloadFile.ContentLength > 0)
                {
                    string _FileName = workForceVM.DownloadFile.FileName;
                    string _path = Path.Combine(Server.MapPath("~/Content/files"), _FileName);
                    if (System.IO.File.Exists(_path))
                        Trace.WriteLine("File aready exists.");
                    else
                        workForceVM.DownloadFile.SaveAs(_path);

                    wf.DownloadFileName = _FileName;
                }

                ViewBag.UploadMessage = "Uploaded successfully";

            }
            catch
            {
                ViewBag.UploadMessage = "Upload error.";
                return View();
            }

            db.WorkForce.Add(wf);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: WorkForces/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkForce workForce = await db.WorkForce.FindAsync(id);
            if (workForce == null)
            {
                return HttpNotFound();
            }

            WorkForceViewModel wfvm = new WorkForceViewModel();
            wfvm.Id = workForce.Id;
            wfvm.PriorityNumber = (int)workForce.PriorityNumber;
            wfvm.Title = workForce.Title;
            wfvm.FileName = workForce.DownloadFileName;

            return View(wfvm);
        }

        // POST: WorkForces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkForceViewModel workForceVM)
        {
            WorkForce wf = new WorkForce();

            using (var ctx = new UVRPEntities1()) {
                wf = ctx.WorkForce.Where(x => x.Id == workForceVM.Id).FirstOrDefault();
                wf.Title = workForceVM.Title;
                wf.PriorityNumber = workForceVM.PriorityNumber;

                try
                {
                    if (workForceVM.DownloadFile != null && workForceVM.DownloadFile.ContentLength > 0)
                    {
                        string _FileName = workForceVM.DownloadFile.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/files"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Trace.WriteLine("File aready exists.");
                        else
                            workForceVM.DownloadFile.SaveAs(_path);

                        wf.DownloadFileName = _FileName;
                    }

                    ViewBag.UploadMessage = "Uploaded successfully";

                }
                catch
                {
                    ViewBag.UploadMessage = "Upload error.";
                    return View();
                }

                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workForceVM);
        }

        // GET: WorkForces/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            WorkForce workForce = await db.WorkForce.FindAsync(id);
            db.WorkForce.Remove(workForce);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: WorkForces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkForce workForce = await db.WorkForce.FindAsync(id);
            db.WorkForce.Remove(workForce);
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
