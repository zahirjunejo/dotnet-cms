using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uvrp.Models;
using CaptchaMvc.HtmlHelpers;
using System.Net;

namespace uvrp.Controllers
{
    public class PostsController : Controller
    {
        // GET: Posts
        public ActionResult Index()
        {
            List<Posts> result;
            using (var ctx = new UVRPEntities1()) {
                result = ctx.Posts.ToList();   
            }

            return View(result);
        }

        public ActionResult Add()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            Posts result;
            using (var ctx = new UVRPEntities1())
            {
                result = ctx.Posts.Find(id);
            }

            return View(result);
        }

        public ActionResult Edit(int id)
        {
            List<SelectListItem> catList = new List<SelectListItem>();



            Posts result;
            using (var ctx=new UVRPEntities1())
            {
                result = ctx.Posts.Find(id);
                foreach (var cat in ctx.Masters.Where(x => x.Type == "Category").ToList())
                {
                    catList.Add(new SelectListItem {Text=cat.Text, Value=cat.Value, Selected= false } );
                }
            }
            
            ViewBag.Cats = new SelectList(catList, "Value", "Text", result.Category);


            return View(result);
        }

        [HttpGet]
        public ActionResult PostDetailsById(int id)
        {
            Posts result;
            using (var ctx = new UVRPEntities1())
            {
                result = ctx.Posts.Find(id);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult GetCategories()
        {
            List<Masters> masters;
            using (var ctx = new UVRPEntities1())
            {
                masters = ctx.Masters.Where(x=> x.Type == "Category").ToList();
            }

            return Json(masters, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult AddPost(PostModel pm) {

            Posts post = new Posts();
            using (var context = new UVRPEntities1()) {
                post.Title = pm.Title;
                post.Content = pm.Content;
                post.Category = pm.Category;
                post.PostStatus = pm.PostStatus;
                post.CreationDate = DateTime.Now.ToString();
                post.EventStartDate = pm.EventStartDate;
                post.EventEndDate = pm.EventEndDate;
                post.Recurring = pm.Recurring;
                post.startTime = pm.startTime;
                post.endTime = pm.endTime;
                post.DaysOfWeek = pm.DaysOfWeek;
                post.EventLocation = pm.Location;
                

                try
                {
                    if (pm.ImageUpload!=null && pm.ImageUpload.ContentLength > 0) {
                        string _FileName = pm.ImageUpload.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Trace.WriteLine("File aready exists.");
                        else
                            pm.ImageUpload.SaveAs(_path);

                        post.ImageUpload = _FileName;
                    }
                }
                catch
                {
                    // ViewBag.UploadMessage = "Upload error";
                    // return View();
                    return Json( new { success=false, message= "Upload error" } , JsonRequestBehavior.AllowGet);
                }

                post.Website = pm.Website;
                post.Organization = pm.Organization;
                post.EventContact = pm.EventContact;
                post.Email = pm.Email;
                post.Phone = pm.Phone;

                post = context.Posts.Add(post);
                context.SaveChanges();
            }
            
            return Json( new { success=true, redirectToUrl = Url.Action("Index", "Posts") } , JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult RequestEvent(PostModel pm)
        {

            Posts post = new Posts();
            using (var context = new UVRPEntities1())
            {
                post.Title = pm.Title;
                post.Content = pm.Content;
                post.Category = pm.Category;
                post.PostStatus = pm.PostStatus;
                post.CreationDate = DateTime.Now.ToString();
                post.EventStartDate = pm.EventStartDate;
                post.EventEndDate = pm.EventEndDate;
                post.Recurring = pm.Recurring;
                post.startTime = pm.startTime;
                post.endTime = pm.endTime;
                post.DaysOfWeek = pm.DaysOfWeek;
                post.EventLocation = pm.Location;


                try
                {
                    if (pm.ImageUpload != null && pm.ImageUpload.ContentLength > 0)
                    {
                        string _FileName = pm.ImageUpload.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Trace.WriteLine("File aready exists.");
                        else
                            pm.ImageUpload.SaveAs(_path);

                        post.ImageUpload = _FileName;
                    }
                }
                catch
                {
                    // ViewBag.UploadMessage = "Upload error";
                    // return View();
                    return Json( new { success=false, message= "Upload error" } , JsonRequestBehavior.AllowGet);
                }

                post.Website = pm.Website;
                post.Organization = pm.Organization;
                post.EventContact = pm.EventContact;
                post.Email = pm.Email;
                post.Phone = pm.Phone;

                post = context.Posts.Add(post);
                context.SaveChanges();
            }

            return Json(new { success = true, redirectToUrl = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPost(PostModel pm) {
            using (var ct = new UVRPEntities1()) {
                Posts foundPost = ct.Posts.Find(pm.Id);
                foundPost.Title = pm.Title;
                foundPost.Content = pm.Content;
                foundPost.Category = pm.Category;
                foundPost.PostStatus = pm.PostStatus;
                foundPost.CreationDate = DateTime.Now.ToString();
                foundPost.EventStartDate = pm.EventStartDate;
                foundPost.EventEndDate = pm.EventEndDate;
                foundPost.Recurring = pm.Recurring;
                foundPost.EventLocation = pm.Location;

                try
                {
                    if (pm.ImageUpload != null && pm.ImageUpload.ContentLength > 0)
                    {
                        string _FileName = pm.ImageUpload.FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                        if (System.IO.File.Exists(_path))
                            Trace.WriteLine("File aready exists.");
                        else
                            pm.ImageUpload.SaveAs(_path);

                        foundPost.ImageUpload = _FileName;
                    }
                }
                catch
                {
                    // ViewBag.UploadMessage = "Upload error";
                    // return View();
                    return Json( new { success=false, message= "Upload error" } , JsonRequestBehavior.AllowGet);
                }

                foundPost.Website = pm.Website;
                foundPost.Organization = pm.Organization;
                foundPost.EventContact = pm.EventContact;
                foundPost.Email = pm.Email;
                foundPost.Phone = pm.Phone;
                foundPost.startTime = pm.startTime;
                foundPost.endTime = pm.endTime;
                foundPost.DaysOfWeek = pm.DaysOfWeek;

                ct.SaveChanges();
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Posts") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetComments(int id) {
            List<Comments> comments;
            using (var ctx = new UVRPEntities1()) {
                comments = ctx.Comments.Where(x => x.PostId == id).ToList();
            }

            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostComment(Comments comment)
        {
            using (var ctx = new UVRPEntities1())
            {
                comment = ctx.Comments.Add(comment);
                ctx.SaveChanges();
            }

            return Json(comment, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            Posts post;
            using (var ctx = new UVRPEntities1())
            {
                post = ctx.Posts.Where(x => x.Id == id).FirstOrDefault();
                ctx.Posts.Remove(post);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Posts post;
            using (var ctx = new UVRPEntities1())
            {
                post = ctx.Posts.Where(x => x.Id == id).FirstOrDefault();
                ctx.Posts.Remove(post);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

        }

    }
}