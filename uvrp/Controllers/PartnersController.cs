using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using uvrp.Models;

namespace uvrp.Controllers
{
    public class PartnersController : Controller
    {
        private UVRPEntities1 db = new UVRPEntities1();
        // GET: Partners
        public ActionResult Index()
        {
            List<Partners> partners;
            using (var ctx = new UVRPEntities1()) {
                partners = ctx.Partners.ToList();
            }

                return View(partners);
        }


        // GET: Partners/Create
        public ActionResult Add()
        {
            return View();
        }
        
        public async Task<ActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Partners partner = await db.Partners.FindAsync(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            
            PartnersVM pvm = new PartnersVM();
            pvm.name = partner.name;
            pvm.description = partner.description;
            pvm.CompanyHomePage = partner.CompanyHomePage;
            
            
            return View(pvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartnersVM pvm)
        {
            try
            {
                String fileName = null;
                if (pvm.Logo?.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(pvm.Logo.FileName);
                    
                    string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                    pvm.Logo.SaveAs(_path);
                    fileName = _FileName;
                }
                using (var ctx = new UVRPEntities1())
                {
                    Partners partner = ctx.Partners.Find(pvm.Id);
                    partner.description = pvm.description;
                    partner.name = pvm.name;
                    if (fileName != null)
                    {
                        partner.Logo = fileName;
                    }

                    partner.CompanyHomePage = pvm.CompanyHomePage;
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

        
        // POST: Partners/Create
        [HttpPost]
        public ActionResult Add(PartnersVM pvm)
        {
            try
            {
                String fileName = "logo-placeholder.jpg";
                if (pvm.Logo?.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(pvm.Logo.FileName);
                    
                    string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                    pvm.Logo.SaveAs(_path);
                    fileName = _FileName;
                }
                using (var ctx = new UVRPEntities1()) {
                    ctx.Partners.Add(new Partners {  
                       name=pvm.name, 
                       description=pvm.description, 
                       Logo = fileName,
                       CompanyHomePage =  pvm.CompanyHomePage
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

        // GET: Partners/Delete/5
        public ActionResult Delete(int id)
        {
            using (var ctx = new UVRPEntities1())
            {
                Partners p = ctx.Partners.Where(x => x.Id == id).FirstOrDefault();
                ctx.Partners.Remove(p);
                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST: Partners/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (var ctx = new UVRPEntities1()) {
                    Partners p = ctx.Partners.Where(x => x.Id == id).FirstOrDefault();
                    ctx.Partners.Remove(p) ;
                    ctx.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
