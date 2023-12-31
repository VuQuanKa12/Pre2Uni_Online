﻿using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewCourse(int IdCourse)
        {
            if (!UserLogin.Instance.Islogin || UserLogin.Instance.RoleID != 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            Course course = CourseDAO.Instance.Courses().Find(x => x.ID == IdCourse);
            return View(course);
        }
        public IActionResult AddNewCourse(Course course)
        {
            if (!UserLogin.Instance.Islogin || UserLogin.Instance.RoleID != 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            var isInsert = CourseDAO.Instance.AddNew(course);
            if (isInsert)
            {
                CourseDAO.Instance.SaveChange();
                LecturerDAO.Instance.SaveChange();
            }
            return RedirectToAction("Course", "Admin");
        }
        public IActionResult UpdateCourse(Course course)
        {
            if (!UserLogin.Instance.Islogin || UserLogin.Instance.RoleID != 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            var isInsert = CourseDAO.Instance.Update(course);
            if (isInsert)
            {
                CourseDAO.Instance.SaveChange();
                LecturerDAO.Instance.SaveChange();
            }
            return RedirectToAction("ViewCourse", "Course", new { IdCourse = course.ID });
        }
        public IActionResult SetStatus(Course course)
        {
            if (!UserLogin.Instance.Islogin || UserLogin.Instance.RoleID != 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            var isInsert = CourseDAO.Instance.setStatus(course);
            if (isInsert)
            {
                CourseDAO.Instance.SaveChange();
                LecturerDAO.Instance.SaveChange();
            }
            return RedirectToAction("Course", "Admin");
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
