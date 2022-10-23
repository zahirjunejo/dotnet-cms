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

namespace uvrp.Controllers
{
    public class MastersController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: Masters
        public async Task<ActionResult> Index()
        {
            return View(await db.Masters.ToListAsync());
        }

        // GET: Masters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Masters masters = await db.Masters.FindAsync(id);
            if (masters == null)
            {
                return HttpNotFound();
            }
            return View(masters);
        }

        // GET: Masters/Create
        public ActionResult Create()
        {
            
            ViewBag.TypeList = new SelectList(
                new List<SelectListItem> { 
                    new SelectListItem { Text="Industry", Value="Industry", Selected=true },
                    new SelectListItem { Text="Category", Value="Category", Selected=false }
                }, "Value", "Text", "Industry");
            
            return View();
        }

        // POST: Masters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Text,Value,Type")] Masters masters)
        {
            if (ModelState.IsValid)
            {
                db.Masters.Add(masters);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(masters);
        }

        // GET: Masters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.TypeList = new SelectList(
                new List<SelectListItem> { 
                    new SelectListItem { Text="Industry", Value="Industry", Selected=true },
                    new SelectListItem { Text="Category", Value="Category", Selected=false }
                }, "Value", "Text", "Industry");
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Masters masters = await db.Masters.FindAsync(id);
            if (masters == null)
            {
                return HttpNotFound();
            }
            return View(masters);
        }

        // POST: Masters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Text,Value,Type")] Masters masters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(masters).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(masters);
        }

        // GET: Masters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Masters masters = await db.Masters.FindAsync(id);
//            if (masters == null)
//            {
//                return HttpNotFound();
//            }
//            return View(masters);
            Masters masters = await db.Masters.FindAsync(id);
            db.Masters.Remove(masters);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // POST: Masters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Masters masters = await db.Masters.FindAsync(id);
            db.Masters.Remove(masters);
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
