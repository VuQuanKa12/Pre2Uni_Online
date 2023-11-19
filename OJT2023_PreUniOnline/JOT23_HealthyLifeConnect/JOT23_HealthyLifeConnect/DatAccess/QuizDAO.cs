using JOT23_Pre2UniOnline.Models;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess
{
	public class QuizDAO : IServiceDAO<Quiz, QuizDAO>
	{
		private List<Quiz> _quizzes = new List<Quiz>();
		private static QuizDAO instance;
		private QuizDAO() { }
		public static QuizDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new QuizDAO();
				}
				return instance;
			}
		}
		public List<Quiz> Quizzes()
		{
			_quizzes.Clear();
			GetAll();
			return _quizzes;
		}
		public void GetAll()
		{
			string query = "SELECT * FROM Quiz";
			DataTable dataTable = ConnectionData.ExecuteQuery(query);
			dataTable = ConnectionData.ExecuteQuery(query);
			foreach (DataRow row in dataTable.Rows)
			{
				Quiz obj = new Quiz()
				{
					IDQuiz = Convert.ToInt32(row["IDQuiz"].ToString().Trim()),
					Question = row["Question"].ToString().Trim(),
					AnswerA = row["AnswerA"].ToString().Trim(),
					AnswerB = row["AnswerB"].ToString().Trim(),
					AnswerC = row["AnswerC"].ToString().Trim(),
					AnswerD = row["AnswerD"].ToString().Trim(),
					CorrectAnswer = row["CorrectAnswer"].ToString().Trim(),
					IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim())
					// Gán các giá trị khác của object từ các cột trong DataTable
				};
				_quizzes.Add(obj);
			}

		}
		public bool Update(Quiz quiz)
		{
			string query = "UPDATE Quiz " +
				" \nSET Question = @question , AnswerA = @A , AnswerB = @B , AnswerC = @C , AnswerD = @D , CorrectAnswer = @CA " +
				" \nWHERE IDQuiz = @IDQuiz";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { quiz.Question, quiz.AnswerA, quiz.AnswerB, quiz.AnswerC, quiz.AnswerD, quiz.CorrectAnswer, quiz.IDQuiz });
			return result;
		}
		public bool Delete(int id)
		{
			string query = "DELETE FROM Quiz WHERE IDQuiz = @id ";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { id });
			return result;
		}
		public bool DeleteAll(int Idcourse)
		{
			string query = "DELETE FROM Quiz WHERE IDCourse = @id ";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { Idcourse });
			return result;
		}
		public bool AddNew(Quiz quiz)
		{
			int ID = UserLogin.Instance.ID;
			string query = "INSERT INTO Quiz " +
				"\nVALUES ( @question , @A , @B , @C , @D , @CA , @IDCourse )";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { quiz.Question, quiz.AnswerA, quiz.AnswerB, quiz.AnswerC, quiz.AnswerD, quiz.CorrectAnswer, quiz.IDCourse });
			return result;
		}
		public void SaveChange()
		{
			_quizzes.Clear();
			GetAll();
		}
	}
}
