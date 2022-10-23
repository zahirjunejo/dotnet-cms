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
    public class HomePageContentsController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();

        // GET: HomePageContents
        public async Task<ActionResult> Index()
        {
            
            using (var ctx =  new UVRPEntities1())
            {
                
                HomePageContent homePageContent = new HomePageContent();
                
                if (ctx.HomePageContent.Any() )
                {
                    homePageContent = ctx.HomePageContent.FirstOrDefault();
                    return View("Edit", homePageContent);
                } else {
                    return View("Create");
                }
            }
            
            return View(await db.HomePageContent.ToListAsync());
        }

        // GET: HomePageContents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomePageContent homePageContent = await db.HomePageContent.FindAsync(id);
            if (homePageContent == null)
            {
                return HttpNotFound();
            }
            return View(homePageContent);
        }

        // GET: HomePageContents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomePageContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Id,Content")] HomePageContent homePageContent)
        {
            if (ModelState.IsValid)
            {
                db.HomePageContent.Add(homePageContent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(homePageContent);
        }

        // GET: HomePageContents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomePageContent homePageContent = await db.HomePageContent.FindAsync(id);
            if (homePageContent == null)
            {
                return HttpNotFound();
            }
            return View(homePageContent);
        }

        // POST: HomePageContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Content")] HomePageContent homePageContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(homePageContent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(homePageContent);
        }

        // GET: HomePageContents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomePageContent homePageContent = await db.HomePageContent.FindAsync(id);
            if (homePageContent == null)
            {
                return HttpNotFound();
            }
            return View(homePageContent);
        }

        // POST: HomePageContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HomePageContent homePageContent = await db.HomePageContent.FindAsync(id);
            db.HomePageContent.Remove(homePageContent);
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
