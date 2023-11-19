using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers
{
    public class DictionaryController : Controller
    {
        public IActionResult Index(string searchDictionary)
        {
            renderData();
            ViewBag.word = searchDictionary;
            SessionUser();
            return View("Views/Home/Dictionary.cshtml");
        }
        private void renderData()
        {
            LecturerDAO.Instance.GetAll();
            StudentDAO.Instance.GetAll();
            CourseDAO.Instance.GetAll();
            QuizDAO.Instance.GetAll();
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
                ViewBag.Role = "Guest";
                ViewBag.login = false;
            }
        }
    }
}
