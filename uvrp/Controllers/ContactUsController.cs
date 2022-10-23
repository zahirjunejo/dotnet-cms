using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace uvrp.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        public ActionResult Index()
        {

            List<Contacts> message; 

            using (var ctx = new UVRPEntities1())
            {
                message = ctx.Contacts.ToList();
            }

            return View(message);
        }

        // GET: ContactUs/Details/5
        public ActionResult Details(int id)
        {
            Contacts selectedMessage;
            using (var ctx = new UVRPEntities1() ) {
                selectedMessage = ctx.Contacts.Where(c => c.Id == id).FirstOrDefault();
            }

            return View(selectedMessage);
        }
        
    }
}
