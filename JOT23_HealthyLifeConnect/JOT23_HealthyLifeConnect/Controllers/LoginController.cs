using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult forgotPass()
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				UserLogin.Instance = null;
			}
			return View();
		}
		public IActionResult Login(AccountLogin acc, string submitButton)
		{
			if (submitButton == "Sutdent")
			{
				if (StudentDAO.Instance.Login(acc) && !acc.Password.Trim().Equals("") && !acc.Email.Trim().Equals(""))
				{
					DateTime nowUtc = DateTime.UtcNow;
					TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Múi giờ của Việt Nam
					DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, vietnamTimeZone);
					var check = (vietnamTime - UserLogin.Instance.DateCreate);
					if (check.TotalDays >= 30 || !UserLogin.Instance.ConfirmEmail)
					{
						return RedirectToAction("ConfirmAcceptAccout", "Register", new { email = UserLogin.Instance.Email, forgot = UserLogin.Instance.ID });
					}

					UserLogin.Instance.Islogin = true;

					UserLogin.Instance.RoleID = 3;
					foreach (var course in UserLogin.Instance.Courses)
					{

						course.Calendars = CourseDAO.Instance.getCalendar(course.ID);
					}

					HttpContext.Session.SetObject("User", UserLogin.Instance);

					return RedirectToAction("Index", "Home");
				}
				else
				{
					TempData["MessError"] = "Pass Or Email was Wrong!!";
					return RedirectToAction("Index", "Login");
				}
			}
			else
			{
				if (LecturerDAO.Instance.login(acc))
				{
					UserLogin.Instance.Islogin = true;
					UserLogin.Instance.RoleID = 2;
					UserLogin.Instance.TimeLogin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
					foreach (var course in UserLogin.Instance.Courses)
					{
						course.Calendars = CourseDAO.Instance.getCalendar(course.ID);
					}
					HttpContext.Session.SetObject("User", UserLogin.Instance);
					return RedirectToAction("Profile", "Home");
				}
				else
				{
					TempData["MessError"] = "Pass Or Email was Wrong!!";
					return RedirectToAction("Index", "Login");
				}

			}
		}
		public IActionResult ResetPassword(string Password)
		{
			var role = HttpContext.Session.GetInt32("role"); // vấn đề nó nằm ở đây, chưa có id á
			if (role == 3)
			{
				Profile.Instance.Password = Password;
				if (StudentDAO.Instance.resetPassword(Profile.Instance))
				{
					Profile.Instance = null;
					TempData["MessSS"] = "Success!!";
					StudentDAO.Instance.SaveChange();
					return RedirectToAction("Index", "Login");
				}
			}
			else
			{
				Profile.Instance.Password = Password;
				if (LecturerDAO.Instance.resetPassword(Profile.Instance))
				{
					Profile.Instance = null;
					TempData["MessSS"] = "Success!!";
					LecturerDAO.Instance.SaveChange();
					return RedirectToAction("Index", "Login");
				}
			}
			TempData["MessError"] = "ERROR in reset password!!";
			return RedirectToAction("Index", "Login");
		}

		public IActionResult Logout()
		{
			UserLogin.Instance.Islogin = false;
			UserLogin.Instance.RoleID = 0;
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}
		private void renderData()
		{

			StudentDAO.Instance.GetAll();

		}

	}

}
