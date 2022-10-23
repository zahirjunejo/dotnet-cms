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
    public class FootersController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: Footers
        public async Task<ActionResult> Index()
        {

            using (var ctx =  new UVRPEntities1())
            {
                Footer footer = new Footer();
                if (ctx.Footer.Any() )
                {
                    footer = ctx.Footer.FirstOrDefault();
                    return View("Edit", footer);
                } else {
                    return View("Create");
                }
            }

//            return View(await db.Footer.ToListAsync());
        }

        // GET: Footers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = await db.Footer.FindAsync(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // GET: Footers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Footers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Id,Content")] Footer footer)
        {
            if (ModelState.IsValid)
            {
                db.Footer.Add(footer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(footer);
        }

        // GET: Footers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = await db.Footer.FindAsync(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // POST: Footers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Content")] Footer footer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(footer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(footer);
        }

        // GET: Footers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = await db.Footer.FindAsync(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // POST: Footers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Footer footer = await db.Footer.FindAsync(id);
            db.Footer.Remove(footer);
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
