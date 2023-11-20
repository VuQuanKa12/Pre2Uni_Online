using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Models;
using JOT23_Pre2UniOnline.Services;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers
{
	public class ForgotController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult ConfirmEmail(Profile pro, string forgot)
		{
			if (pro != null)
			{
				//Check submit email là từ Student hay Lecture
				//Xg phải lấy đc id từ email đó rồi truyền vào pro.RoleID
				if (int.TryParse(Request.Form["Role"], out int roleID))
				{
					pro.RoleID = roleID;
					
					
				}


				HttpContext.Session.SetInt32("role", pro.RoleID);
				Profile.Instance = pro;
				string email = pro.Email;
				if(roleID == 3)
				{
					pro.ID = StudentDAO.Instance.getIDbyEmail(email);
				}
				else
				{
					pro.ID = LecturerDAO.Instance.getIDbyEmail(email);
				}
				
				(bool, int) afterSend = Email.Instance.sendOTP(email, forgot);
				bool isSend = afterSend.Item1;
				int OTP = afterSend.Item2;
				if (isSend)
				{
					ViewBag.Email = email;
					ViewBag.OTP = OTP;
					return View("Views/Login/ConfirmEmail.cshtml", forgot);
				}
			}
			TempData["MessError"] = "ERROR in register form let try again!!";
			return RedirectToAction("Index", "Login");
		}
		 
	}
}
