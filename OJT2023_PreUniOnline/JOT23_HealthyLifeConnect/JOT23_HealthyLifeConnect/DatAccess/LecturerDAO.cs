using JOT23_Pre2UniOnline.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess
{
	public class LecturerDAO : IUserDAO<LecturerDAO>
	{
		private List<Lecturer> _lecturers = new List<Lecturer>();
		private static LecturerDAO insctance;
		public static LecturerDAO Instance
		{
			get
			{
				if (insctance == null)
				{
					insctance = new LecturerDAO();
				}
				return insctance;
			}
		}

		public List<Lecturer> Lecturers()
		{
			GetAll();
			return _lecturers;
		}

		public void GetAll()
		{
			_lecturers.Clear();
			if (_lecturers.Count == 0)
			{
				string query = "SELECT * from Lecturer join Account on Lecturer.IDAccount = Account.IDAccount and Account.Status = 1 ";
				DataTable dataTable = ConnectionData.ExecuteQuery(query);
				foreach (DataRow row in dataTable.Rows)
				{
					Lecturer obj = new Lecturer()
					{
						ID = Convert.ToInt32(row["ID"].ToString().Trim()),
						IDAccount = Convert.ToInt32(row["IDAccount"].ToString().Trim()),
						FirstName = row["FirstName"].ToString().Trim(),
						LastName = row["LastName"].ToString().Trim(),
						RoleID = Convert.ToInt32(row["RoleID"].ToString().Trim()),
						Email = row["Email"].ToString().Trim(),
						Password = row["Password"].ToString().Trim(),
						Phone = row["Phone"].ToString().Trim(),
						Avatar = row["Avatar"].ToString().Trim(),
						Major = row["Major"].ToString().Trim(),
						DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
						Address = row["Address"].ToString().Trim(),
						Status = Boolean.Parse(row["Status"].ToString().Trim()),
						Courses = getCourse(Convert.ToInt32(row["ID"].ToString().Trim())),
						Quizs = GetQuizzes(Convert.ToInt32(row["ID"].ToString().Trim()))


						// Gán các giá trị khác của object từ các cột trong DataTable
					};

					_lecturers.Add(obj);
				}
			}
			_lecturers.Reverse();
		}
		public bool login(AccountLogin acc)
		{
			string query = "select Lecturer.*, Account.* from Lecturer join Account on Lecturer.IDAccount = Account.IDAccount and Account.Email = @Email and Account.Password = @Password and Account.Status = 1 ";
			DataTable tb = ConnectionData.ExecuteQuery(query, new object[] { acc.Email, acc.Password });
			foreach (DataRow row in tb.Rows)
			{
				UserLogin obj = new UserLogin()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					FirstName = row["FirstName"].ToString().Trim(),
					LastName = row["LastName"].ToString().Trim(),
					RoleID = Convert.ToInt32(row["RoleID"].ToString().Trim()),
					Password = row["Password"].ToString().Trim(),
					Email = row["Email"].ToString().Trim(),
					Phone = row["Phone"].ToString().Trim(),
					Avatar = row["Avatar"].ToString().Trim(),
					DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
					Status = Boolean.Parse(row["Status"].ToString().Trim()),
					Courses = getCourse(Convert.ToInt32(row["ID"].ToString().Trim())),
				};
				UserLogin.Instance = obj;
			}
			if (tb.Rows.Count == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		public List<Course> getCourse(int id)
		{
			List<Course> courses = new List<Course>();
			string query = "SELECT * FROM Course WHERE IDLecturer = @id";
			DataTable dataTable = ConnectionData.ExecuteQuery(query, new object[] { id });
			foreach (DataRow row in dataTable.Rows)
			{
				Course obj = new Course()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					Title = row["Title"].ToString().Trim(),
					Detail = row["Detail"].ToString().Trim(),
					Status = bool.Parse(row["Status"].ToString().Trim()),
					Price = float.Parse(row["Price"].ToString().Trim()),
					Sale = float.Parse(row["Sale"].ToString().Trim())
				};

				courses.Add(obj);
			}
			return courses;
		}
		public List<Quiz> GetQuizzes(int id)
		{
			List<Quiz> quizzes = new List<Quiz>();
			string query = "SELECT * FROM Lecturer";
			DataTable dataTable = ConnectionData.ExecuteQuery(query);
			return quizzes;
		}
		public bool Update(Profile pro)
		{
			int ID = UserLogin.Instance.ID;
			string query = "update Account set FirstName = @FirstName , LastName = @LastName , Password = @Pasword , Email = @Email , Phone = @Phone , DOB = @DOB from Account join Lecturer on Account.IDAccount = Lecturer.IDAccount where Lecturer.ID = @Id ";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { pro.FirstName, pro.LastName, pro.Password, pro.Email, pro.Phone , pro.DOB, pro.ID });
			return result;
		}
		public bool uploadAvatar(string name, int id)
		{
			string query = "Update Account set Avatar = @Avatar from Account join Lecturer on Account.IDAccount = Lecturer.IDAccount and Lecturer.ID = @id";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { name, id });
			return result;
		}
		public void SaveChange()
		{
			_lecturers.Clear();
			GetAll();
		}
		public bool resetPassword(Profile pro)
		{
			bool result = false;
			string query = "update Account set Email = @Email , Password = @password from Account join Lecturer on Account.IDAccount = Lecturer.IDAccount and Lecturer.ID =  @id ";
			result = ConnectionData.ExecuteUpdate(query, new object[] {  pro.Email, pro.Password , pro.ID });
			return result;
		}
		public int getIDbyEmail(string email)
		{
			int id = 0;
			string query = " select * from Lecturer join Account on Lecturer.IDAccount = Account.IDAccount and Account.Email = @email ";
			DataTable datatable = ConnectionData.ExecuteQuery(query, new object[] { email });
			foreach (DataRow row in datatable.Rows)
			{
				id = Convert.ToInt32(row["ID"]);
			}
			return id;
		}
		public bool Delete(int id)
		{
			string query = "UPDATE Course SET IDLecturer = NULL, Status = 0 WHERE IDLecturer = @IDlecturer ";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { id });

			query = "DELETE FROM tblLecturer WHERE IDLecturer = @IDlecturer ";
			result = ConnectionData.ExecuteUpdate(query, new object[] { id });
			return result;
		}
		public bool ChangePass(int Id, string Pass)
		{
			bool result = false;
			string query = "UPDATE Lecturer SET Password = @pass WHERE ID = @IDlecturer ";
			result = ConnectionData.ExecuteUpdate(query, new object[] { Pass, Id });
			return result;
		}
		public bool AddNew(Profile le)
		{
			bool result = false;
			string query = "INSERT INTO Lecturer VALUES " +
				"( @first , @Last , 2 , @email , @pass , @phone , 'Lecturer.png' ) ";
			result = ConnectionData.ExecuteUpdate(query, new object[] { le.FirstName, le.LastName, le.Email, le.Password, le.Phone });
			return result;
		}
	}
}
