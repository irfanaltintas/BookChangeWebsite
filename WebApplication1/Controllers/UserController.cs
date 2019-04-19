using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        bookstoreDB db = new bookstoreDB();
        // GET: User
        public ActionResult Index(int id)
        { 
            var user = db.Users.Where(u => u.user_id == id).SingleOrDefault();
            if (Convert.ToInt32(Session["userid"]) != user.user_id)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var login = db.Users.Where(u => u.email == user.email).SingleOrDefault();
            if(login.email == user.email && login.password == user.password)
            {
                Session["userid"] = login.user_id;
                Session["email"] = login.email;
                Session["authorityid"] = login.authority_id;
                Session["username"] = login.u_name;
                Session["credit"] = login.credit;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Warning = "Check your information";
                return View();
            }
           
        }
        public ActionResult Logout()
        {
            Session["userid"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null)
                {
                    WebImage img = new WebImage(photo.InputStream);
                    FileInfo photoinfo = new FileInfo(photo.FileName);

                    string newphoto = Guid.NewGuid().ToString() + photoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UserFoto/" + newphoto);
                    user.photo = "/Uploads/UserFoto/" + newphoto;

                    user.authority_id = 2;
                    user.credit = 0;
                    db.Users.Add(user);
                    db.SaveChanges();
                    Session["userid"] = user.user_id;
                    Session["email"] = user.email;
                    Session["username"] = user.u_name;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("photo", "chose a photo");
                }
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                        Response.End();
                    }

                  
                }
                
            }
            return View(user);
        }
        public ActionResult Edit(int id)
        {
            var user = db.Users.Where(u => u.user_id == id).SingleOrDefault();
            if(Convert.ToInt32(Session["userid"]) != user.user_id)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user, int id, HttpPostedFileBase photo)
        {  
            if (ModelState.IsValid)
            {
                var newuser = db.Users.Where(u => u.user_id == id).SingleOrDefault();
                if(photo != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(newuser.photo)))
                    {
                        System.IO.File.Delete(Server.MapPath(newuser.photo));
                    }
                    WebImage img = new WebImage(photo.InputStream);
                    FileInfo photoinfo = new FileInfo(photo.FileName);
                    string newphoto = Guid.NewGuid().ToString() + photoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UserFoto/" + newphoto);
                    newuser.photo = "/Uploads/UserFoto/" + newphoto;
                }
                    newuser.u_name = user.u_name;
                    newuser.u_surname = user.u_surname;
                    newuser.email = user.email;
                    newuser.password = user.password;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home", new { id = newuser.user_id });
                  
            }
            return View();
        }
        public ActionResult UserProfil(int id)
        {
            var user = db.Users.Where(u => u.user_id == id).SingleOrDefault();
            return View(user);
        }
        

    }
}