using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers
{
	public class ProfileController : Controller
	{
		private readonly IWebHostEnvironment _environment;

		public ProfileController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}
		public IActionResult Index()
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				UserLogin.Instance = null;
			}
			SessionUser();
			return View();
		}
		public IActionResult EditInformation(Profile pro)
		{
			if (pro != null)
			{
				if (pro.RoleID == 3)
				{
					bool checkEdited = StudentDAO.Instance.Update(pro);
					if (checkEdited)
					{
						int idUser = pro.ID;
						pro.RoleID = 3;
						UserLogin.updateIfor(idUser, 3);

						StudentDAO.Instance.SaveChange();
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					bool checkEdited = LecturerDAO.Instance.Update(pro);
					if (checkEdited)
					{
						pro.RoleID = 2;
						int idUserLgin = pro.ID;
						UserLogin.updateIfor(idUserLgin, 2);

						LecturerDAO.Instance.SaveChange();
					}
				}
				UserLogin.Instance.Islogin = true;
				UserLogin.Instance.Courses.ForEach(course => course.Calendars = CourseDAO.Instance.getCalendar(course.ID));
				HttpContext.Session.Clear();
				HttpContext.Session.SetObject("User", UserLogin.Instance);
				SessionUser();
				return RedirectToAction("Profile", "Home");
			}
			return RedirectToAction("Index", "Home");
		}
		public IActionResult UploadAvatar(IFormFile image)
		{
			var user = HttpContext.Session.GetObject<UserLogin>("User");
			if (user == null)
			{
				UserLogin.Instance = null;
			}

			renderData();
			SessionUser();
			var roleID = user.RoleID;
			var id = user.ID;
			if (image != null && image.Length > 0)
			{
				ViewBag.fail = "not fail";
				var fileName = $"{roleID}-{id}{Path.GetExtension(image.FileName)}";
				var path = "";
				if (id == 2)
				{
					path = Path.Combine(_environment.WebRootPath, $"img\\Avatar\\Lecturer", fileName);
				}
				else
				{
					path = Path.Combine(_environment.WebRootPath, $"img\\Avatar\\Student", fileName);
				}
				//var fileName = Path.GetFileName(image.FileName);

				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					image.CopyTo(fileStream);
				}
				if (user.RoleID == 3)
				{
					StudentDAO.Instance.uploadAvatar(fileName, user.ID);
				}
				else
				{
					LecturerDAO.Instance.uploadAvatar(fileName, user.ID);
				}
				user.Avatar = fileName;
				HttpContext.Session.SetObject("User", user);
			}
			else
			{
				ViewBag.fail = "fail";
			}
			SessionUser();
			return RedirectToAction("Profile", "Home");
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
			var user = HttpContext.Session.GetObject<UserLogin>("User"); // Debug ở đây xem
			if (user != null)
			{
				ViewBag.Role = user.RoleID;
				ViewBag.login = true;
				ViewBag.user = user;
				ViewBag.Avatar = user.Avatar;

			}
		}
	}
}
