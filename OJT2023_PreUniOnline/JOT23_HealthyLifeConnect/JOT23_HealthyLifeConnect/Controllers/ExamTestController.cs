using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Extend;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace JOT23_Pre2UniOnline.Controllers
{
	public class ExamTestController : Controller
	{
		private readonly IWebHostEnvironment _environment;
		public ExamTestController(IWebHostEnvironment environment)
		{
			_environment = environment;
		}
		public IActionResult Index(int idCourse)
		{

			Course course = CourseDAO.Instance.Courses().FirstOrDefault(x => x.ID == idCourse);
			ViewBag.Course = course.Title;
			ViewBag.IDCourse = course.ID;
			
			SessionUser();
			return View();
		}
		public IActionResult PassExam(int idExam)
		{
            var user = HttpContext.Session.GetObject<UserLogin>("User");
            ExamTest exam = ExamDAO.Instance.ExamTests().FirstOrDefault(x => x.ID == idExam);
			SessionUser();
			ViewBag.Exam = exam;
			return View();
		}
		public IActionResult RamdomQuestion(int IdCourse, int numQues , int idExamTest)
		{
			ExamDAO.Instance.RandomAddQues(IdCourse, numQues, idExamTest);
			SessionUser();
			return RedirectToAction("ListExamTest", "ExamTest");
		}
	public IActionResult Create(ExamTest exam , int IDCourse)
		{
			SessionUser();
			ExamTest e = ExamDAO.Instance.AddExam(exam);
				ViewBag.IDExamTest = e.ID;
				ViewBag.num = e.NumQuestion;
			ViewBag.nameExam = exam.NameTest;
			ViewBag.Pass = exam.Password;
			ViewBag.IDCourse = IDCourse;


				return View("Views/ExamTest/DetailExam.cshtml", exam);
				
		}
	
		public IActionResult ListExamTest()
		{
            var user = HttpContext.Session.GetObject<UserLogin>("User");
            ViewBag.IDLec = user.ID;
			SessionUser();
			return View();
		}
		public IActionResult ListExamTestStudent()
		{
			var user = HttpContext.Session.GetObject<UserLogin>("User");
			ViewBag.idstudent = user.ID;
			SessionUser();
			return View();
		}
		public IActionResult LecturerDoQuiz(int idExam )
		{
			List<QuestionExam> list = new List<QuestionExam>();
			list = ExamDAO.Instance.GetQuestionByIDExam(idExam);
			ExamTest examtest = ExamDAO.Instance.ExamTests().Where(x => x.ID == idExam).FirstOrDefault();
			ViewBag.examName = examtest.NameTest.ToString();
			ViewBag.idexam = examtest.ID;
			ViewBag.obExam = examtest;
			SessionUser();
			return View(list);
		}

		public IActionResult importFileExam(IFormFile fileExcel, int IDExamTest, int NumQuestion)
		{
			if (fileExcel != null && fileExcel.Length > 0 && (Path.GetExtension(fileExcel.FileName) == ".xls" || Path.GetExtension(fileExcel.FileName) == ".xlsx"))
			{
				var fileName = $"FileExam{Path.GetExtension(fileExcel.FileName)}";

				var path = Path.Combine(_environment.WebRootPath, $"File", fileName);
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					fileExcel.CopyTo(fileStream);
				}
				readInsertQuiz(IDExamTest, NumQuestion);
				
			}
			SessionUser();
			return RedirectToAction("ListExamTest", "ExamTest");
		}
		private void readInsertQuiz(int IDExamTest, int NumQuestion)
		{
			string filePath = "wwwroot/File/FileExam.xlsx"; // Đường dẫn tới file Excel trên server
			FileInfo fileInfo = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using (ExcelPackage package = new ExcelPackage(fileInfo))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên

				int startRow = worksheet.Dimension.Start.Row + 1; // Dòng bắt đầu chứa dữ liệu
				int endRow = worksheet.Dimension.End.Row; // Dòng kết thúc chứa dữ liệu
				int count = 0;
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
						QuestionExam quiz = new QuestionExam() { Question = question, AnswerA = answerA, AnswerB = answerB, AnswerC = answerC, AnswerD = answerD, CorrectAnswer = answerCR, IDExamTest = IDExamTest };
						ExamDAO.Instance.AddnewQuestion(quiz);
						count++;
					if(count == NumQuestion)
						{
							break;
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Invalid data at row " + row + ": " + ex.Message);
					}
				}
			}
		}


		public IActionResult SubmitExam(Dictionary<int, string> answer , int IDExamTest , int NumberQuestion)
		{
			if (HttpContext.Session.GetObject<UserLogin>("User") == null)
			{
				UserLogin.Instance = null;
			}
			
			var score = CalculatorScore(answer);
			ViewBag.Score = score;
			ViewBag.IDExamTest = IDExamTest;
			ViewBag.numquestion = NumberQuestion;
			float point = (float)NumberQuestion / 10 * score;
			ViewBag.diem = point;
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
				bool isCorrect = ExamDAO.Instance.questionExams().Find(q => q.Id == idQuiz).CorrectAnswer == answer;
				if (isCorrect)
				{
					score++;
				}
			}
			SessionUser();
			return score;
		}
		public IActionResult AddResultStudent(float Point , int IDStudent , int IDExam , int numCorrect)
		{
			
			SessionUser();
			if (ExamDAO.Instance.AddResultExamStudent(Point, IDStudent, IDExam, numCorrect))
			{
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("ListExamTestStudent", "ExamTest");
		}
		public IActionResult LecturerViewResult(int idExam , string examName)
		{
			SessionUser();
			List<ResultQuiz> listresults = new List<ResultQuiz>();
			ViewBag.nameExam = examName;
			listresults = ExamDAO.Instance.GetListResult(idExam);
            listresults.OrderByDescending(result => result.Point).ToList();
            return View(listresults);
        }


        [HttpPost]
        public JsonResult CheckStatusExam(int idExam, int idStudent)
        {
            // Thực hiện kiểm tra trạng thái của kỳ thi
            var result = ExamDAO.Instance.CheckStatusExam(idExam , idStudent);

            return Json(result);
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
