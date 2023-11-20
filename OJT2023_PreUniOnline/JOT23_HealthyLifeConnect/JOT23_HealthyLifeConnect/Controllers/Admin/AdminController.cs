using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.DatAccess.Admin;
using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace JOT23_Pre2UniOnline.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(AccountLogin acc)
        {
            renderData();
            if (AdminDAO.Instence().Login(acc))
            {
                UserLogin.Instance.Email = acc.Email;
                UserLogin.Instance.Password = acc.Password;
                UserLogin.Instance.RoleID = 1;
                UserLogin.Instance.Islogin = true;
                HttpContext.Session.SetObject("ADMIN", UserLogin.Instance);
                return RedirectToAction("ShowAccountData", "Admin", new { page = 0 });
            }
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult Student(int page = 0, int max = 0)
        {
            if (HttpContext.Session.GetObject<UserLogin>("ADMIN") == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            List<Student> subList = new List<Student>();
            
                subList = StudentDAO.Instance.AdminGetList();
            subList.OrderBy(obj => obj.ID).ToList();


			ViewBag.crPage = page;
            return View(@"Views/Admin/AdminPage.cshtml", subList);
        }
        public IActionResult Lecturer(int page = 0, int max = 1)
        {
            if (HttpContext.Session.GetObject<UserLogin>("ADMIN") == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            List<Lecturer> subList = new List<Lecturer>();
            if (page != (max - 1))
            {
                subList = LecturerDAO.Instance.Lecturers().GetRange(8 * page, 8);
            }
            else
            {
                subList = LecturerDAO.Instance.Lecturers().Skip(8 * page).ToList();
            }
            ViewBag.crPage = page;

            return View(subList);
        }
        public IActionResult Course(int page = 0, int max = 1)
        {
            if (HttpContext.Session.GetObject<UserLogin>("ADMIN") == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            List<Course> subList = new List<Course>();
            if (page != (max - 1))
            {
                subList = CourseDAO.Instance.Courses().GetRange(8 * page, 8);
            }
            else
            {
                subList = CourseDAO.Instance.Courses().Skip(8 * page).ToList();
            }
            ViewBag.crPage = page;
            return View(subList);
        }
        public IActionResult Calendar()
        {
            renderData();
            if (HttpContext.Session.GetObject<UserLogin>("ADMIN") == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            List<Calendar> list = new List<Calendar>();
            list = CalendarDAO.Instance.Calendars();

			return View(list);
        }
        public IActionResult Nontification()
        {
            if (HttpContext.Session.GetObject<UserLogin>("ADMIN") == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }
		public IActionResult ShowAccountData()
		{
			if (HttpContext.Session.GetObject<UserLogin>("ADMIN") == null)
			{
				return RedirectToAction("Index", "Admin");
			}
			return View();
		}

		[HttpPost]
		public List<object> GetSalesData()
		{
			List<object> data = new List<object>();
			DateTime currentDate = DateTime.Now.Date;
			List<DateTime> labels = Enumerable.Range(0, 7)
			.Select(i => currentDate.AddDays(-i))
			.ToList();
			data.Add(labels);
			List<int> SalesNumber = new List<int>();
			foreach (DateTime date in labels)
			{
				int count = 0;
				count = CountAccountsCreatedToday(date);
				SalesNumber.Add(count);
			}
			data.Add(SalesNumber);
			return data;
		}

		static int CountAccountsCreatedToday(DateTime day)
		{
			List<Student> students = new List<Student>();
			students = StudentDAO.Instance.Student();
			// Lấy ngày hiện tại
			DateTime today = DateTime.Now.Date;
			// Đếm số lượng tài khoản được tạo trong ngày hiện tại
			int count = students.Count(account => account.DateCreate.Date == day);
			return count;
		}


		[HttpPost]
		public List<object> GetCourseData()
		{
			List<object> data = new List<object>();
			DateTime currentMonthStart = DateTime.Now.Date.AddDays(1 - DateTime.Now.Day); // Ngày đầu tháng hiện tại
			List<DateTime> labels = Enumerable.Range(0, 7)
				.Select(i => currentMonthStart.AddMonths(-i))
				.ToList();
			data.Add(labels);

			List<int> Number = new List<int>();
			foreach (DateTime monthStart in labels)
			{
				int count = 0;
				count = CountCoursePurchaseMonth(monthStart);
				Number.Add(count);
			}
			data.Add(Number);
			return data;
		}

		static int CountCoursePurchaseMonth(DateTime monthStart)
		{
			List<CourseOfStudent> courseOfStudents = CourseOfStudentDAO.Instanse.CourseOfStudents();
			DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1); // Ngày cuối tháng

			int count = courseOfStudents.Count(courseofstudent =>
				courseofstudent.PurchaseDate.Date >= monthStart.Date &&
				courseofstudent.PurchaseDate.Date <= monthEnd.Date);

			return count;
		}

        [HttpPost]
        public List<object> GetTotalPriceByDate()
        {
            List<object> data = new List<object>();

            DateTime currentDate = DateTime.Now.Date;
            List<DateTime> labels = Enumerable.Range(0, 7)
                .Select(i => currentDate.AddDays(-i))
                .ToList();
            data.Add(labels);

            List<double> SalesRevenue = new List<double>();
            foreach (DateTime date in labels)
            {
                double revenue = GetRevenueForDay(date);
                SalesRevenue.Add(revenue);
            }
            data.Add(SalesRevenue);

            return data;
        }

        static double GetRevenueForDay(DateTime day)
        {
            List<TransactionHistory> transactions = new List<TransactionHistory>();
            transactions = TransactionDAO.Instanse.Transactions();

            double revenue = transactions
                .Where(transaction => transaction.Date.Date == day)
                .Sum(transaction => transaction.Price);

            return revenue;
        }
        public IActionResult LogOut()
        {
            UserLogin.Instance.Email = "";
            UserLogin.Instance.Password = "";
            UserLogin.Instance.RoleID = 0;
            UserLogin.Instance.Islogin = false;
            return RedirectToAction("Index", "Admin");
        }

        private void renderData()
        {
            LecturerDAO.Instance.GetAll();
            StudentDAO.Instance.GetAll();
            CourseDAO.Instance.GetAll();
        }
    }
}
