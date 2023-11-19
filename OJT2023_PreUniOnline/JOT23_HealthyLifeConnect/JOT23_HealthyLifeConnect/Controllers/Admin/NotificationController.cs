using JOT23_Pre2UniOnline.DataAccess;

using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult addnewNotification(Notification notification)
        {
            NotificationDAO.Instanse.AddNew(notification);
            NotificationDAO.Instanse.SaveChange();
            return RedirectToAction("Nontification", "Admin");
        }
        public IActionResult delete(int idNotification)
        {
            NotificationDAO.Instanse.Delete(idNotification);
            NotificationDAO.Instanse.SaveChange();
            return RedirectToAction("Nontification", "Admin");
        }

        public IActionResult ViewNotification()
        {
            SessionUser();
            return View();
        }
        private void SessionUser()
        {
            var user = HttpContext.Session.GetObject<UserLogin>("User");
            if (user != null)
            {
                ViewBag.Role = user.RoleID;
                ViewBag.login = true;
                ViewBag.user = user;
            }
            else
            {
                ViewBag.Role = 0;
                ViewBag.login = false;
            }
        }
    }
}
