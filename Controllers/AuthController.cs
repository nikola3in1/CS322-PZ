using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS322_PZ.Models;
using PusherServer;

namespace CS322_PZ.Controllers
{
    public class AuthController : Controller
    {


        private Pusher pusher;

        public AuthController()
        {
            var options = new PusherOptions
            {
                Cluster = "eu",
                Encrypted = true
            };
            pusher = new Pusher(
                "937376",
                "81b971a4e62af906bec5",
                "12fd29bfd337f2060c09",
                options);
        }
        [HttpPost]
        public ActionResult Login()
        {

            string user_name = Request.Form["username"];

            if (user_name.Trim() == "")
            {
                return Redirect("/");
            }

            using (var db = new Models.ChatContext())
            {
                string databaseName = db.Database.Connection.Database;
                Console.Write(databaseName);

                User user = db.Users.FirstOrDefault(u => u.name == user_name);

                if (user == null)
                {
                    user = new User { name = user_name,created_at = DateTime.Now };

                    db.Users.Add(user);
                    db.SaveChanges();
                }

                Session["user"] = user;
            }

            return Redirect("/chat");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return Redirect("/");
        }

        public JsonResult AuthForChannel(string channel_name, string socket_id)
        {
            if (Session["user"] == null)
            {
                return Json(new { status = "error", message = "User is not logged in" });
            }

            var currentUser = (Models.User)Session["user"];

            if (channel_name.IndexOf("presence") >= 0)
            {

                var channelData = new PresenceChannelData()
                {
                    user_id = currentUser.id.ToString(),
                    user_info = new
                    {
                        id = currentUser.id,
                        name = currentUser.name
                    },
                };

                var presenceAuth = pusher.Authenticate(channel_name, socket_id, channelData);

                return Json(presenceAuth);

            }

            if (channel_name.IndexOf(currentUser.id.ToString()) == -1)
            {
                return Json(new { status = "error", message = "User cannot join channel" });
            }

            var auth = pusher.Authenticate(channel_name, socket_id);

            return Json(auth);


        }
    }
}