﻿using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers
{
	public class MeetingRoomController : Controller
	{
		private static int idroom;
		public IActionResult Index(int idCourse)
		{
			var user = HttpContext.Session.GetObject<UserLogin>("User");
			var check = new Course();
			check = user.Courses.FirstOrDefault(x => x.ID == idCourse);
			if (user == null || check == null)
			{
				return RedirectToAction("Index", "Home");
			}
			idroom = idCourse;
			return RedirectToAction("Room", "MeetingRoom");
		}
		public IActionResult Room()
		{
			var user = HttpContext.Session.GetObject<UserLogin>("User");
			if (user == null)
			{
				return RedirectToAction("Index", "Home");
			}
			if (user != null)
			{
				ViewBag.roomid = idroom;
				ViewBag.nameUser = user.FirstName + " " + user.LastName;
				ViewBag.UerID = user.ID;
				ViewBag.role = user.RoleID;
			}
			return View();
		}
	}
}
