using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CS322_PZ.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult DeleteAll()
        {
            using (var db = new Models.ChatContext())
            {

                Console.WriteLine("DELETE ALL MOFOS");
                db.Users.RemoveRange(db.Users);
                db.Conversations.RemoveRange(db.Conversations);
                db.SaveChanges();
            }

            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}