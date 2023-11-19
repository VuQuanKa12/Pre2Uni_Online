using JOT23_Pre2UniOnline.Models;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JOT23_Pre2UniOnline.DatAccess
{
    public class ExamDAO : IServiceDAO<ExamTest, ExamDAO>
    {
        private List<ExamTest> _exams = new List<ExamTest>();
        private static ExamDAO instance;
        private ExamDAO() { }
        public static ExamDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExamDAO();
                }
                return instance;
            }
        }
        public List<ExamTest> ExamTests()
        {
            
            _exams.Clear();
            GetAll();
            return _exams.OrderBy(c => c.ID).ToList();
        }
        public List<QuestionExam> questionExams()
        {
            List<QuestionExam> list = new List<QuestionExam>();
            string query = "select * from QuestionExam ";
            DataTable tb = ConnectionData.ExecuteQuery(query);
            foreach (DataRow row in tb.Rows)
            {
                QuestionExam ques = new QuestionExam()
                {
                    Id = Convert.ToInt32(row["ID"].ToString().Trim()),
                    Question = row["Question"].ToString().Trim(),
                    AnswerA = row["AnswerA"].ToString().Trim(),
					AnswerB = row["AnswerB"].ToString().Trim(),
					AnswerC = row["AnswerC"].ToString().Trim(),
					AnswerD = row["AnswerD"].ToString().Trim(),
					CorrectAnswer = row["CorrectAnswer"].ToString().Trim()
				};
                list.Add(ques);
            }
            return list;
        }
        public ExamTest AddExam(ExamTest obj)
        {
            ExamTest examTest = null;
            string query = "insert into ExamTest(NameTest,IDCourse,TimeStart,TimeEnd,DateExam,NumQuestion,Password)\r\nvalues( @Name , @IdCourse , @TimeStart , @TimeEnd , @DateStart , @Num , @pass )\r\nDECLARE @idExamTest INT;\r\nSET @idExamTest = SCOPE_IDENTITY();\r\nselect * from ExamTest where ID = @idExamTest";

            DataTable dataTable = ConnectionData.ExecuteQuery(query, new object[] { obj.NameTest, obj.IDCourse, obj.TimeStart, obj.TimeEnd, obj.DateExam, obj.NumQuestion , obj.Password });
            foreach (DataRow row in dataTable.Rows)
            {
                ExamTest exam = new ExamTest()
                {
                    ID = Convert.ToInt32(row["ID"].ToString().Trim()),
                    NameTest = row["NameTest"].ToString().Trim(),
                    IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim()),
                    TimeStart = TimeSpan.Parse(row["TimeStart"].ToString().Trim()),
					TimeEnd = TimeSpan.Parse(row["TimeEnd"].ToString().Trim()),
                    DateExam = DateTime.Parse(row["DateExam"].ToString().Trim()),
                    NumQuestion = Convert.ToInt32(row["NumQuestion"].ToString().Trim()),
                    Password = row["Password"].ToString().Trim()
				};
                examTest = exam;

			}
            return examTest;

		}
        public ExamTest GetexambyID(int idExam)
        {
            ExamTest exam = new ExamTest();
            string query = "select * from ExamTest where ID = @id ";
            DataTable dataTable = ConnectionData.ExecuteQuery(query, new object[] { idExam });
            foreach (DataRow row in dataTable.Rows)
            {
                ExamTest obj = new ExamTest()
                {
                    ID = Convert.ToInt32(row["ID"].ToString().Trim()),
                    NameTest = row["NameTest"].ToString().Trim(),
                    IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim()),
                    TimeStart = TimeSpan.Parse(row["TimeStart"].ToString().Trim()),
                    TimeEnd = TimeSpan.Parse(row["TimeEnd"].ToString().Trim()),
                    DateExam = DateTime.Parse(row["DateExam"].ToString().Trim()),
                    NumQuestion = Convert.ToInt32(row["NumQuestion"].ToString().Trim()),
                    Password = row["Password"].ToString().Trim()
                };
                exam = obj;

            }
            return exam;
        }
        public List<ExamTest> GetExamByIDCourse(int idCourse)
        {
            List<ExamTest>  list = new List<ExamTest>();
            string query = "select * from ExamTest where IDCourse = @id ";
            DataTable tb = ConnectionData.ExecuteQuery(query, new object[] {idCourse});
            foreach (DataRow row in tb.Rows)
            {
                ExamTest exam = new ExamTest()
                {
                    ID = Convert.ToInt32(row["ID"].ToString().Trim()),
                    NameTest = row["NameTest"].ToString().Trim(),
                    IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim()),
                    TimeStart = TimeSpan.Parse(row["TimeStart"].ToString().Trim()),
                    TimeEnd = TimeSpan.Parse(row["TimeEnd"].ToString().Trim()),
                    DateExam = DateTime.Parse(row["DateExam"].ToString().Trim()),
                    NumQuestion = Convert.ToInt32(row["NumQuestion"].ToString().Trim()),
                    Password = row["Password"].ToString().Trim()
                };
               list.Add(exam);

            }
            return list.OrderByDescending(c => c.ID).ToList();
        }
        public List<ExamTest> GetExamByIDLecturer(int ID)
        {
            List<ExamTest> list = new List<ExamTest>();
            string query = "select ExamTest.* from Lecturer join Course on Lecturer.ID = Course.IDLecturer join ExamTest on Course.ID = ExamTest.IDCourse where Lecturer.ID = @ID ";
            DataTable tb = ConnectionData.ExecuteQuery(query, new object[] { ID });
            foreach (DataRow row in tb.Rows)
            {
                ExamTest exam = new ExamTest()
                {
                    ID = Convert.ToInt32(row["ID"].ToString().Trim()),
                    NameTest = row["NameTest"].ToString().Trim(),
                    IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim()),
                    TimeStart = TimeSpan.Parse(row["TimeStart"].ToString().Trim()),
                    TimeEnd = TimeSpan.Parse(row["TimeEnd"].ToString().Trim()),
                    DateExam = DateTime.Parse(row["DateExam"].ToString().Trim()),
                    NumQuestion = Convert.ToInt32(row["NumQuestion"].ToString().Trim()),
                    Password = row["Password"].ToString().Trim()
                };
                list.Add(exam);
            }
            return list.OrderByDescending(c => c.ID).ToList();
        }
        public List<Quiz> GetAllQuestbyIDExam(int IdCourse, int numQues)
        {
            int count = 0;
            List<Quiz> list = new List<Quiz> ();
            string query = "select * from Quiz where IDCourse = @IdCourse ";
            DataTable tb = ConnectionData.ExecuteQuery(query, new object[] { IdCourse });
            foreach (DataRow row in tb.Rows)
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
                list.Add(obj);
                count++;
                if (count == numQues)
                {
                    break;
                }
            }
            return list;
        }
        public List<QuestionExam> GetQuestionByIDExam(int id)
        {
            List<QuestionExam> questionExams = new List<QuestionExam>();
            string query = "select * from QuestionExam where IDExamTest = @id ";
            DataTable tb = ConnectionData.ExecuteQuery(query, new object[] { id });
            foreach (DataRow row in tb.Rows)
            {
                QuestionExam obj = new QuestionExam()
                {
                    Id = Convert.ToInt32(row["ID"].ToString().Trim()),
                    Question = row["Question"].ToString().Trim(),
                    AnswerA = row["AnswerA"].ToString().Trim(),
                    AnswerB = row["AnswerB"].ToString().Trim(),
                    AnswerC = row["AnswerC"].ToString().Trim(),
                    AnswerD = row["AnswerD"].ToString().Trim(),
                    CorrectAnswer = row["CorrectAnswer"].ToString().Trim(),
                    IDExamTest = Convert.ToInt32(row["IDExamTest"].ToString().Trim())

                };
                questionExams.Add(obj);
            }
            return questionExams;
        }
            public bool RandomAddQues (int IdCourse, int numQues, int idExamTest)
        {
            bool result = false;
            List<Quiz> list = new List<Quiz>(); 
            list = GetAllQuestbyIDExam(IdCourse, numQues);
			foreach (Quiz obj in list)
            {
                QuestionExam objExam = new QuestionExam() { 
                Question = obj.Question,
                AnswerA = obj.AnswerA,
                AnswerB = obj.AnswerB,
                AnswerC = obj.AnswerC,
                AnswerD = obj.AnswerD,
                CorrectAnswer = obj.CorrectAnswer,
                IDExamTest = idExamTest
                };
                 result =  AddnewQuestion(objExam);

			}
            return result;
        }
        public bool AddnewQuestion(QuestionExam quiz)
        {
            string query = "insert into QuestionExam (Question, AnswerA, AnswerB, AnswerC, AnswerD, CorrectAnswer, IDExamTest) values ( @Question , @AnserA , @AnsswerB , @AnswerC , @AnswerD , @CorrectAnswer , @IDExamTest )";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { quiz.Question, quiz.AnswerA, quiz.AnswerB, quiz.AnswerC, quiz.AnswerD, quiz.CorrectAnswer, quiz.IDExamTest});
			return result;
		}
       
        public bool Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public void GetAll()
        {
			_exams.Clear();
				string query = "select * from ExamTest";
				DataTable tb = ConnectionData.ExecuteQuery(query);
            foreach (DataRow row in tb.Rows)
            {
                ExamTest examt = new ExamTest() {
                    ID = Convert.ToInt32(row["ID"].ToString().Trim()),
               NameTest = row["NameTest"].ToString().Trim(),

               TimeStart = TimeSpan.Parse(row["TimeStart"].ToString().Trim()),
                TimeEnd = TimeSpan.Parse(row["TimeEnd"].ToString().Trim()),
               Password = row["Password"].ToString().Trim(),
               NumQuestion = Convert.ToInt32(row["NumQuestion"].ToString().Trim()),
               DateExam = DateTime.Parse(row["DateExam"].ToString().Trim()),
              IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim())
            };
				_exams.Add(examt);
				}
			
		}

         public bool AddResultExamStudent(float diem , int idstudent , int examtest, int correctAnswer)
        {
			bool result = false;
			string query = "insert into ResultQuiz (IDExamTest , IDStudent , Point , NumberAnswerCorrect) values( @idexam , @idstudent , @point , @numcorrect )";
            result = ConnectionData.ExecuteUpdate(query, new object[] { examtest, idstudent, diem, correctAnswer });
            return result;
        }


        public List<ResultQuiz> GetListResult(int idExam)
        {
            List<ResultQuiz> list = new List<ResultQuiz>();
            string query = "select * from ResultQuiz join Student on ResultQuiz.IDStudent = Student.ID join Account on Student.IDAccount = Account.IDAccount join ExamTest on ResultQuiz.IDExamTest = ExamTest.ID where ResultQuiz.IDExamTest = @idExam ";
            DataTable tb = ConnectionData.ExecuteQuery(query, new object[] {idExam});
            foreach (DataRow row in tb.Rows)
            {
                ResultQuiz result = new ResultQuiz()
                {
                    ID = Convert.ToInt32(row["IDStudent"].ToString().Trim()),
                    IDExamTest = Convert.ToInt32(row["IDExamTest"].ToString().Trim()),
                    IDStudent = Convert.ToInt32(row["IDStudent"].ToString().Trim()),
                    Point = float.Parse(row["Point"].ToString().Trim()),
                    NumberAnswerCorrect = Convert.ToInt32(row["NumberAnswerCorrect"].ToString().Trim()),
                    FirstName = row["FirstName"].ToString().Trim(),
                    LastName = row["LastName"].ToString().Trim(),
                    NameExam = row["NameTest"].ToString().Trim()

                };
                list.Add(result);
            }
            return list;
        }

        public bool CheckStatusExam(int idExam, int idStudent)
        {
            string query = "select * from ResultQuiz where IDStudent = @idStudent and IDExamTest = @idExam ";
            
            DataTable tb = ConnectionData.ExecuteQuery(query, new object[] { idStudent,idExam });
            if(tb.Rows.Count == 0)
            {
                return true;
            }
            return false;

        }
        public void SaveChange()
        {
			
				_exams.Clear();
				GetAll();
		
		}

        public bool Update(ExamTest obj)
        {
            throw new NotImplementedException();
        }

		public bool AddNew(ExamTest obj)
		{
			throw new NotImplementedException();
		}
	}
}
