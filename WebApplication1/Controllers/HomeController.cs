using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers   
{
    public class HomeController : Controller
    {
        bookstoreDB db = new bookstoreDB();
        // GET: Home
        public ActionResult Index()
        {
            
            var book = db.Books.OrderByDescending(b => b.book_id).ToList();
            return View(book);
        }

        public ActionResult SearchBook( string ara = null)
        {
            var aranan = db.Books.Where(b => b.b_name.Contains(ara)).ToList();
            return View(aranan.OrderByDescending(b => b.b_name));
        }

        public ActionResult CategoryBook(int id)
        {
            var kitaplar = db.Books.Where(b => b.Category.category_id == id).ToList();
            return View(kitaplar);
        }

        public ActionResult BookDetails(int id)
        {
            var book = db.Books.Where(b => b.book_id == id).SingleOrDefault();
            if(book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        public ActionResult CategoryPartial()
        {
            return View(db.Categories.ToList());
        }
        [HttpPost]
        public JsonResult CreateComment(string newcomment, int bookid)
        {
            var userid = Session["userid"];
            if(newcomment == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
                
            }
                db.Comments.Add(new Comment {user_id = Convert.ToInt32(userid),book_id=bookid,context = newcomment,date = DateTime.Now});
                db.SaveChanges();

            return Json(false,JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteComment(int id)
        {
            var userid = Session["userid"];
            var comment = db.Comments.Where(c => c.comment_id == id).SingleOrDefault();
            var book = db.Books.Where(b => b.book_id == comment.book_id).SingleOrDefault();
            if (comment.user_id == Convert.ToInt32(userid))
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return RedirectToAction("BookDetails", "Home", new { id = book.book_id });
            }
            else
            {
                return HttpNotFound();
            }
           
        }
       
    }
   
}