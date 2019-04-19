using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminUserController : Controller
    {
        private bookstoreDB db = new bookstoreDB();

        // GET: AdminUser
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Authority);
            return View(users.ToList());
        }

        // GET: AdminUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: AdminUser/Create
        public ActionResult Create()
        {
            ViewBag.authority_id = new SelectList(db.Authorities, "authority_id", "authority1");
            return View();
        }

        // POST: AdminUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,u_name,u_surname,email,password,photo,authority_id,credit")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.authority_id = new SelectList(db.Authorities, "authority_id", "authority1", user.authority_id);
            return View(user);
        }

        // GET: AdminUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.authority_id = new SelectList(db.Authorities, "authority_id", "authority1", user.authority_id);
            return View(user);
        }

        // POST: AdminUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,u_name,u_surname,email,password,photo,authority_id,credit")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.authority_id = new SelectList(db.Authorities, "authority_id", "authority1", user.authority_id);
            return View(user);
        }

        // GET: AdminUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AdminUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);

            foreach (var item in user.Books.ToList())
            {
                if(item.user_id == id)
                {
                    item.user_id = null;
                }
            }
            foreach (var item in user.Comments.ToList())
            {
                if (item.user_id == id)
                {
                    item.user_id = null;
                }
            }

            db.Users.Remove(user);
            db.SaveChanges();
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
