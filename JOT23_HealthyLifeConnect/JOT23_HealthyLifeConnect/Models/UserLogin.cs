using JOT23_Pre2UniOnline.DatAccess;
using System.Data;

namespace JOT23_Pre2UniOnline.Models
{
	public class UserLogin : Student
	{
		private static UserLogin instance = new UserLogin();
		public static UserLogin Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new UserLogin();
				}
				return instance;
			}
			set { instance = value; }
		}// => instance ?? (instance = new UserLogin());
		public TimeSpan TimeLogin { get; set; }
		public bool Islogin { get; set; } = false;

		public static void updateIfor(int id, int type)
		{
			string query = "";
			DataTable tb;
			switch (type)
			{
				case 3:
					query = "select * From Student join Account on Student.IDAccount = Account.IDAccount where Student.ID = @ID and Account.Status =1";
					tb = ConnectionData.ExecuteQuery(query, new object[] { id });
					foreach (DataRow row in tb.Rows)
					{
						UserLogin obj = new UserLogin()
						{
							ID = Convert.ToInt32(row["ID"].ToString().Trim()),
							FirstName = row["FirstName"].ToString().Trim(),
							LastName = row["LastName"].ToString().Trim(),
							RoleID = Convert.ToInt32("3"),
							Password = row["Password"].ToString().Trim(),
							Email = row["Email"].ToString().Trim(),
							Phone = row["Phone"].ToString().Trim(),
							Gender = row["Gender"].ToString().Trim(),
							DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
							ConfirmEmail = Boolean.Parse(row["ConfirmEmail"].ToString().Trim()),
							DateCreate = DateTime.Parse(row["DateCreate"].ToString().Trim()),
							Avatar = row["Avatar"].ToString().Trim(),
							Status = Boolean.Parse(row["Status"].ToString().Trim()),
							Courses = StudentDAO.Instance.GetsCourses(Convert.ToInt32(row["ID"].ToString()))
							

						};
						UserLogin.Instance = obj;
					}
					break;
				case 2:
					query = "select * from Lecturer join Account on Lecturer.IDAccount = Account.IDAccount where Lecturer.ID = @ID and Account.Status = 1";
					tb = ConnectionData.ExecuteQuery(query, new object[] { id });
					foreach (DataRow row in tb.Rows)
					{
						UserLogin obj = new UserLogin()
						{
							ID = Convert.ToInt32(row["ID"].ToString().Trim()),
							FirstName = row["FirstName"].ToString().Trim(),
							LastName = row["LastName"].ToString().Trim(),
							RoleID = Convert.ToInt32("2"),
							Email = row["Email"].ToString().Trim(),
							Password = row["Password"].ToString().Trim(),
							Phone = row["Phone"].ToString().Trim(),
							Avatar = row["Avatar"].ToString().Trim(),
							DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
							Status = Boolean.Parse(row["Status"].ToString().Trim()),
							Courses = LecturerDAO.Instance.getCourse(Convert.ToInt32(row["ID"].ToString()))
						};
						UserLogin.Instance = obj;
					}
					break;
			}
		}
	}
}
