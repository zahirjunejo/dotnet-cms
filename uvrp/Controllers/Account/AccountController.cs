using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uvrp.Models;
using System.Web.Security;
using Scrypt;
using System.Diagnostics;

namespace uvrp.Controllers
{
    [AllowAnonymous]

    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Membership model)
        {
            
            using (var context = new UVRPEntities1())
            {

                var existingUser = context.Users.SingleOrDefault(x => x.UserName.Equals(model.UserName));

                if (existingUser == null){
                    ModelState.AddModelError("", "Invalid username and password");
                    return View();
                }

                var hashCode = existingUser.SaltKey;
//                var encodingPasswordString = Helpers.PasswordEncHelper.EncodePassword(model.Password, hashCode);

                bool passwordMatches = context.Users.Any(x=> x.UserName == model.UserName && x.Password.Equals(model.Password));

                if (passwordMatches){
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    return RedirectToAction("Index", "Dashboard");
                } else {
                    ModelState.AddModelError("", "Invalid username and password");
                    return View();
                }
                
            }
        }


        // SignUp
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User model)
        {
            
            model.Role = "member";
            using (var context = new UVRPEntities1())
            {

                bool userExists = context.Users.Any(x => x.UserName == model.UserName);
                if (userExists) {
                    ModelState.AddModelError("", "User already exists.");
                    return View();
                } else {
//                    var saltKey = Helpers.PasswordEncHelper.GenerateSaltKey(10);
//                    var password = Helpers.PasswordEncHelper.EncodePassword(model.Password, saltKey);
//                    model.SaltKey = saltKey;
                    context.Users.Add(model);
                    context.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}