﻿using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace JOT23_Pre2UniOnline.Controllers
{
	public class QuizController : Controller
	{
		private readonly IWebHostEnvironment _environment;

		public QuizController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}
		public IActionResult Index()
		{
			SessionUser();
			return View();
		}
		private static List<Quiz> quizs = new List<Quiz>();
		public IActionResult Quiz(int courseChoise = 1)
		{
			QuizDAO.Instance.Quizzes();
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				UserLogin.Instance = null;
			}
			if (HttpContext.Session.GetObject<UserLogin>("User") != null)
			{
				quizs.Clear();
				Random rand = new Random();
				HashSet<int> usedIndexes = new HashSet<int>(); // Sử dụng HashSet để theo dõi các chỉ số đã sử dụng
				var quizCourse = QuizDAO.Instance.Quizzes().Where(x => x.IDCourse == courseChoise).ToList();

				if (courseChoise != 0)
				{
					int numberQuiz = quizCourse.Count;
					if (numberQuiz > 10)
					{
						for (int i = 0; i < 10; i++)
						{
							int numberRandom;
							do
							{
								numberRandom = rand.Next(0, numberQuiz);
							}
							while (usedIndexes.Contains(numberRandom)); // Lặp lại cho đến khi có chỉ số chưa được sử dụng
							Quiz subQuiz = QuizDAO.Instance.Quizzes()[numberRandom];

							subQuiz = QuizDAO.Instance.Quizzes().Where(x => x.IDCourse == courseChoise).ToList()[numberRandom];

							usedIndexes.Add(numberRandom); // Đánh dấu chỉ số đã sử dụng

							quizs.Add(subQuiz);
						}
					}
					else
					{
						quizs = quizCourse;
					}
				}
				else
				{

					if (QuizDAO.Instance.Quizzes().Count > 5)
					{
						quizs = QuizDAO.Instance.Quizzes().Take(5).ToList();
					}
					else
					{
						quizs = QuizDAO.Instance.Quizzes();
					}
				}
			}
			else
			{

				if (QuizDAO.Instance.Quizzes().Count > 5)
				{
					quizs = QuizDAO.Instance.Quizzes().Take(5).ToList();
				}
				else
				{
					quizs = QuizDAO.Instance.Quizzes();
				}

			}
			SessionUser();
			return View(quizs);
		}
		public IActionResult QuizGuest(int idcoursesGuest)
		{
			List<Quiz> resut = new List<Quiz>();
			if (int.TryParse(Request.Form["courseChoiseGuest"], out int coursesGuest))
			{
				idcoursesGuest = coursesGuest;

			}
			resut = QuizDAO.Instance.Quizzes().Where(x => x.IDCourse == idcoursesGuest).Take(5).ToList();

			return View(resut);
		}
		public IActionResult SubmitQuiz(Dictionary<int, string> answer)
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				UserLogin.Instance = null;
			}
			var score = CalculatorScore(answer);
			ViewBag.Score = score;
			SessionUser();
			return View(answer);
		}
		private int CalculatorScore(Dictionary<int, string> answers)
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				UserLogin.Instance = null;
			}
			int score = 0;
			foreach (KeyValuePair<int, string> entry in answers)
			{
				int idQuiz = entry.Key;
				string answer = entry.Value;
				bool isCorrect = QuizDAO.Instance.Quizzes().Find(q => q.IDQuiz == idQuiz).CorrectAnswer == answer;
				if (isCorrect)
				{
					score++;
				}
			}
			SessionUser();
			return score;
		}
		public IActionResult DetailQuiz(int IDCourse)
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null || HttpContext.Session.GetObject<UserLogin>("User").RoleID != 2)
			{
				return RedirectToAction("Index", "Home");
			}
			List<Quiz> quizzes = new List<Quiz>();
			quizzes = QuizDAO.Instance.Quizzes().Where(x => x.IDCourse == IDCourse).ToList();
			SessionUser();
			ViewBag.CourseTitle = CourseDAO.Instance.Courses().Find(x => x.ID == IDCourse).Title;
			ViewBag.Idcourse = IDCourse;
			return View(quizzes);
		}

		public IActionResult EditQuiz(string btnQuiz, Quiz quiz)
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				UserLogin.Instance = null;
			}
			if (btnQuiz == "Edit")
			{
				if (QuizDAO.Instance.Update(quiz))
				{
					QuizDAO.Instance.SaveChange();
					RedirectToAction("DetailQuiz", "Quiz", new { IDCourse = quiz.IDCourse });
				}
			}
			else
			{
				if (QuizDAO.Instance.Delete(quiz.IDQuiz))
				{
					QuizDAO.Instance.SaveChange();
					RedirectToAction("DetailQuiz", "Quiz", new { IDCourse = quiz.IDCourse });
				}
			}
			SessionUser();
			return RedirectToAction("DetailQuiz", "Quiz", new { IDCourse = quiz.IDCourse });
		}
		public IActionResult Addnew(Quiz quiz)
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				UserLogin.Instance = null;
			}
			QuizDAO.Instance.AddNew(quiz);
			QuizDAO.Instance.SaveChange();
			CourseDAO.Instance.SaveChange();
			SessionUser();
			return RedirectToAction("DetailQuiz", "Quiz", new { IDCourse = quiz.IDCourse });
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
				ViewBag.Role = 0;
				ViewBag.login = false;
			}
		}
		public IActionResult importFileQuiz(IFormFile fileExcel, int CourseID)
		{
			if (fileExcel != null && fileExcel.Length > 0 && (Path.GetExtension(fileExcel.FileName) == ".xls" || Path.GetExtension(fileExcel.FileName) == ".xlsx"))
			{
				var fileName = $"FileQuiz{Path.GetExtension(fileExcel.FileName)}";

				var path = Path.Combine(_environment.WebRootPath, $"File", fileName);
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					fileExcel.CopyTo(fileStream);
				}
				readInsertQuiz(CourseID);
				QuizDAO.Instance.SaveChange();
			}
			SessionUser();
			return RedirectToAction("DetailQuiz", "Quiz", new { IDCourse = CourseID });
		}
		private void readInsertQuiz(int Idcourse)
		{
			string filePath = "wwwroot/File/FileQuiz.xlsx"; // Đường dẫn tới file Excel trên server
			FileInfo fileInfo = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using (ExcelPackage package = new ExcelPackage(fileInfo))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên

				int startRow = worksheet.Dimension.Start.Row + 1; // Dòng bắt đầu chứa dữ liệu
				int endRow = worksheet.Dimension.End.Row; // Dòng kết thúc chứa dữ liệu

				for (int row = startRow; row <= endRow; row++)
				{
					try
					{
						int column = 1;
						string question = worksheet.Cells[row, column++].Value.ToString();
						string answerA = worksheet.Cells[row, column++].Value.ToString();
						string answerB = worksheet.Cells[row, column++].Value.ToString();
						string answerC = worksheet.Cells[row, column++].Value.ToString();
						string answerD = worksheet.Cells[row, column++].Value.ToString();
						string answerCR = worksheet.Cells[row, column++].Value.ToString();
						Quiz quiz = new Quiz() { Question = question, AnswerA = answerA, AnswerB = answerB, AnswerC = answerC, AnswerD = answerD, CorrectAnswer = answerCR, IDCourse = Idcourse };
						QuizDAO.Instance.AddNew(quiz);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Invalid data at row " + row + ": " + ex.Message);
					}
				}
			}
		}
		public IActionResult removeAllQuiz(int CourseID)
		{
			QuizDAO.Instance.DeleteAll(CourseID);
			QuizDAO.Instance.SaveChange();
			return RedirectToAction("DetailQuiz", "Quiz", new { IDCourse = CourseID });
		}
	}
}
