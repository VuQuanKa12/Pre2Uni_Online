using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult searchStudentByLastname(string LastName)
        {
            List<Student> list = StudentDAO.Instance.AdminReadListStudent().Where(x => x.LastName.Contains(LastName)).ToList();
            return View("Views/Admin/AdminPage.cshtml", list);
        }
        public IActionResult StudentAction(int idStudent, string btnStudentAdmin)
        {
            if (!UserLogin.Instance.Islogin || UserLogin.Instance.RoleID != 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            if (btnStudentAdmin == "View")
            {

            }
            else
            {
                var isLock  = StudentDAO.Instance.Lock(idStudent);
                if (isLock)
                {
                    StudentDAO.Instance.SaveChange();
                }
            }
            return RedirectToAction("Student", "Admin", new { page = 0 });
        }
        public IActionResult ViewStudent(int idStudent)
        {
            if (!UserLogin.Instance.Islogin || UserLogin.Instance.RoleID != 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            if (!UserLogin.Instance.Islogin)
            {
                return RedirectToAction("Index", "Admin");
            }
            Student student = StudentDAO.Instance.Student().Find(x => x.ID == idStudent);
            student.Courses.ForEach(x => x.Calendars = CourseDAO.Instance.getCalendar(x.ID));
            return View("Views/Admin/ViewStudent.cshtml", student);
        }
        public IActionResult UnlockStudent(int idStudent)
        {
            var isUnlock = StudentDAO.Instance.Unlock(idStudent);
            if (isUnlock)
            {
                StudentDAO.Instance.SaveChange();
            }
            return RedirectToAction("Student", "Admin", new { page = 0 });
        }
        public IActionResult updateStudent(Profile pro)
        {
            if (!UserLogin.Instance.Islogin || UserLogin.Instance.RoleID != 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            StudentDAO.Instance.Update(pro);
            StudentDAO.Instance.SaveChange();
            return RedirectToAction("ViewStudent", "Student", new { idStudent = pro.ID });
        }
        public IActionResult ChangePassword(int Id, string Password)
        {
            var check = StudentDAO.Instance.ChangePass(Id, Password);
            ViewBag.Message = "ERROR!!";
            if (check)
            {
                ViewBag.Message = "Change new password was SUCCESS!!";
            }
            return RedirectToAction("Profile", "Home");
        }
        private void renderData()
        {
            LecturerDAO.Instance.GetAll();
            StudentDAO.Instance.GetAll();
            CourseDAO.Instance.GetAll();
            QuizDAO.Instance.GetAll();
        }
    }
}
