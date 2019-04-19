using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class UserBookController : Controller
    {
        bookstoreDB db = new bookstoreDB();
        // GET: AdminBook
        public ActionResult Index()
        {
            var books = db.Books.ToList();
            return View(books);
        }

        // GET: AdminBook/Details/5
        public ActionResult Details(int id)
        {
            var book = db.Books.Where(b => b.book_id == id).SingleOrDefault();
            return View(book);
        }

        // GET: AdminBook/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "cname");
            return View();
        }

        // POST: AdminBook/Create
        [HttpPost]
        public ActionResult Create(Book book, string tags, HttpPostedFileBase photo)
        {

            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    WebImage img = new WebImage(photo.InputStream);
                    FileInfo photoinfo = new FileInfo(photo.FileName);

                    string newphoto = Guid.NewGuid().ToString() + photoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/BookFoto/" + newphoto);
                    book.photo = "/Uploads/BookFoto/" + newphoto;

                }
                if (tags != null)
                {
                    string[] tagarray = tags.Split(',');
                    foreach (var i in tagarray)
                    {
                        var newtag = new Tag { tag_name = i };
                        db.Tags.Add(newtag);
                        book.Tags.Add(newtag);
                    }
                }
                book.user_id = Convert.ToInt32(Session["userid"]);
                book.request = 0;
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);

        }

        // GET: AdminBook/Edit/5
        public ActionResult Edit(int id)
        {
            var book = db.Books.Where(b => b.book_id == id).SingleOrDefault();
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "cname", book.category_id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: AdminBook/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase photo, Book book)
        {
            try
            {
                var editingbook = db.Books.Where(b => b.book_id == id).SingleOrDefault();
                if (photo != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(editingbook.photo)))
                    {
                        System.IO.File.Delete(Server.MapPath(editingbook.photo));
                    }
                    WebImage img = new WebImage(photo.InputStream);
                    FileInfo photoinfo = new FileInfo(photo.FileName);

                    string newphoto = Guid.NewGuid().ToString() + photoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/BookFoto/" + newphoto);
                    editingbook.photo = "/Uploads/BookFoto/" + newphoto;
                    editingbook.b_name = book.b_name;
                    editingbook.aboutbook = book.aboutbook;
                    editingbook.writer = book.writer;
                    editingbook.p_house = book.p_house;
                    editingbook.category_id = book.category_id;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminBook/Delete/5
        public ActionResult Delete(int id)
        {
            var book = db.Books.Where(b => b.book_id == id).SingleOrDefault();
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: AdminBook/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var book = db.Books.Where(b => b.book_id == id).SingleOrDefault();
                if (book == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(book.photo)))
                {
                    System.IO.File.Delete(Server.MapPath(book.photo));
                }
                foreach (var i in book.Comments.ToList())
                {
                    db.Comments.Remove(i);
                }
                foreach (var i in book.Tags.ToList())
                {
                    db.Tags.Remove(i);
                }
                db.Books.Remove(book);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult TakeBook(int id)
        {
            var book = db.Books.Where(b => b.book_id == id).SingleOrDefault();
            return View(book);
        }
        

        public ActionResult AddCredit(int userid)
        {
            var user = db.Users.Where(u => u.user_id == userid).SingleOrDefault();
            user.credit += 1;
            db.SaveChanges();
            return View();
        }
        public ActionResult MinusCredit(int userid, int bookid)
        {
            var book = db.Books.Where(b => b.book_id == bookid).SingleOrDefault();
            var user = db.Users.Where(u => u.user_id == userid).SingleOrDefault();
            book.request += 1;
            user.credit -= 1;    
            db.SaveChanges();
            return View();
        }

    }
}