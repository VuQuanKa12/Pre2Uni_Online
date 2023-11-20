using JOT23_HealthyLifeConnect.Models;
using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace JOT23_HealthyLifeConnect.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			//renderData();
			var user = HttpContext.Session.GetObject<UserLogin>("User");
			if (user != null)
			{
				if (user.RoleID == 2)
				{
					return RedirectToAction("Profile", "Home");
				}
			}
			SessionUser();
			return View();
		}
		public IActionResult CourseDetail(int id)
		{
			Course course = CourseDAO.Instance.Courses().FirstOrDefault(x => x.ID == id);
			course.Comments = CourseDAO.Instance.getComment(course.ID);
			SessionUser();
			return View(@"Views/Home/DetailCourse.cshtml", course);

		}
		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult Profile()
		{
			var user = HttpContext.Session.GetObject<UserLogin>("User");
			if (user == null)
			{
				return RedirectToAction("Index", "Login");
			}
			List<TransactionHistory> trans = new List<TransactionHistory>();

			if (user.RoleID == 3)
			{
				trans = TransactionDAO.Instanse.Transactions().Where(x => x.IDStudent == user.ID).ToList();

			}
			SessionUser();
			return View(trans);
		}
		public IActionResult Commemt(int IDStudent, int IDCourse, string comment)
		{
			Course course = new Course();
			if (CourseDAO.Instance.Comment(IDStudent, IDCourse, comment))
			{
				course = CourseDAO.Instance.Courses().FirstOrDefault(x => x.ID == IDCourse);
				course.Comments = CourseDAO.Instance.getComment(course.ID);
				CourseDAO.Instance.SaveChange();
				LecturerDAO.Instance.SaveChange();
			}
			SessionUser();
			return View(@"Views/Home/DetailCourse.cshtml", course);
		}

		public IActionResult ViewLecturer(int IDLecturer)
		{
			Lecturer lecturer = LecturerDAO.Instance.Lecturers().Find(x => x.ID == IDLecturer);
			lecturer.Courses.ForEach(x => x.Calendars = CourseDAO.Instance.getCalendar(x.ID));
			SessionUser();
			return View(lecturer);
		}
		public IActionResult StudentCourse(int IDCourse)
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				return RedirectToAction("Index", "Login");
			}
			SessionUser();
			List<Student> list = CourseDAO.Instance.StudentCourse(IDCourse);
			ViewBag.IdCourse = IDCourse;
			return View(list);
		}
		public IActionResult searchStudentByLastname(string LastName, int IDCourse)
		{
			List<Student> list = CourseDAO.Instance.StudentCourse(IDCourse).Where(x => x.LastName.Contains(LastName)).ToList();
			SessionUser();
			return View("Views/Home/StudentCourse.cshtml", list);
		}
		public IActionResult About()
		{
			SessionUser();
			return View();
		}
		public IActionResult Course()
		{
			SessionUser();
			return View();
		}
		public IActionResult Contact()
		{
			SessionUser();
			return View();
		}
		public IActionResult Calendar()
		{
			SessionUser();
			return View();
		}
		private void renderData()
		{
			LecturerDAO.Instance.GetAll();
			StudentDAO.Instance.GetAll();
			CourseDAO.Instance.GetAll();
			QuizDAO.Instance.GetAll();
			TransactionDAO.Instanse.Transactions();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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