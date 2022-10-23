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
    public class TeamsController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: Teams
        public async Task<ActionResult> Index()
        {
            return View(await db.Team.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Team.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TeamsViewModel teamVM)
        {
            try
            {
                if (teamVM.Image.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(teamVM.Image.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                    teamVM.Image.SaveAs(_path);
                }

                using (var ctx = new UVRPEntities1()) {
                    ctx.Team.Add(new Team {  
                        Name= teamVM.Name, 
                        Designation= teamVM.Designation, 
                        Image = Path.GetFileName(teamVM.Image.FileName) 
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

        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Team.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Designation,Image")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Team team = await db.Team.FindAsync(id);
//            if (team == null)
//            {
//                return HttpNotFound();
//            }
//            return View(team);
            Team team = await db.Team.FindAsync(id);
            db.Team.Remove(team);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Team team = await db.Team.FindAsync(id);
            db.Team.Remove(team);
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
