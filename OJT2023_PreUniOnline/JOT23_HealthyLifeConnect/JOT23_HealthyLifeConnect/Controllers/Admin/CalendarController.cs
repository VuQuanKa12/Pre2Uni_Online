using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace JOT23_Pre2UniOnline.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		
		public IActionResult DeleteTimeCalendar(int IdCalendar)
        {
			if (CalendarDAO.Instance.Delete(IdCalendar))
			{
				CalendarDAO.Instance.SaveChange();
				return RedirectToAction("Calendar", "Admin");
			}
			return RedirectToAction("Calendar", "Admin");
		}

		public IActionResult AddNewCalendar(int IDCourse, TimeSpan TimeStart , TimeSpan TimeEnd , string selectedDays)
        {
            bool result = false;
         List<int> days = selectedDays.Split(',').Select(int.Parse).ToList();
            for(int i=0; i<days.Count; i++)
            {
               result= CalendarDAO.Instance.AddNewCalendar(IDCourse, TimeStart, TimeEnd, days[i]);

			}
        if (result)
            {
                CalendarDAO.Instance.SaveChange();
            }
            return RedirectToAction("Calendar", "Admin");
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
