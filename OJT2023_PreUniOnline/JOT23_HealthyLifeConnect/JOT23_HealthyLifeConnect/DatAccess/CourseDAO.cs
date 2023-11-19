using JOT23_HealthyLifeConnect.Services;
using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess
{
	public class CourseDAO : IServiceDAO<Course, CourseDAO>
	{
		private List<Course> _coureses = new List<Course>();
		private static CourseDAO insctance;
		public static CourseDAO Instance
		{
			get
			{
				if (insctance == null)
				{
					insctance = new CourseDAO();
				}
				return insctance;
			}
		}

		public List<Course> Courses()
		{
			if (_coureses.Count == 0)
			{
				GetAll();
			}
			return _coureses.OrderByDescending(c => c.ID).ToList();
		}
		
		public void GetAll()
		{
			_coureses.Clear();
			if (_coureses.Count == 0)
			{
				string query = "DECLARE @CurrentDate DATETIME = GETDATE();\r\n\r\nSELECT *\r\nFROM Course\r\nWHERE @CurrentDate BETWEEN DayStart AND DayEnd;";
				DataTable dataTable = ConnectionData.ExecuteQuery(query);
				foreach (DataRow row in dataTable.Rows)
				{

					Course obj = new Course()
					{
						ID = Convert.ToInt32(row["ID"].ToString().Trim()),
						Title = row["Title"].ToString().Trim(),
						Detail = row["Detail"].ToString().Trim(),
						Price = float.Parse(row["Price"].ToString().Trim()),
						Sale = float.Parse(row["Sale"].ToString().Trim()),
						Status = bool.Parse(row["Status"].ToString().Trim()),
						Quantity = Convert.ToInt32(row["Quantity"].ToString().Trim()),
						Image= row["Image"].ToString().Trim(),
						DayStart = DateTime.Parse(row["DayStart"].ToString().Trim()),
						DayEnd = DateTime.Parse(row["DayEnd"].ToString().Trim()),
						Calendars = getCalendar(Convert.ToInt32(row["ID"].ToString().Trim())),
						Students = StudentCourse(Convert.ToInt32(row["ID"].ToString().Trim())),
						
						
					};
					if (row["IDLecturer"].ToString().Trim() != "")
					{  
						obj.Lecturer = LecturerDAO.Instance.Lecturers().FirstOrDefault(x => x.ID == Convert.ToInt32(row["IDLecturer"].ToString().Trim()));
					}

					_coureses.Add(obj);
				}
			}
		}
		public List<Course> getCourseLecturer(int idLecturer)
		{
			List<Course> lisst = new List<Course>();
			List<Course> list = new List<Course>();
			list = CoursebyIDLecture();
			foreach(Course c in list)
			{
				if(c.IDLecturer == idLecturer)
				{
					Course course = new Course()
					{
						ID = Convert.ToInt32(c.ID),
						Title = c.Title,
						Detail = c.Detail,
						Price = c.Price,
						Sale = c.Sale,
						Status = c.Status,
						Quantity = c.Quantity,
						Image = c.Image,
						DayStart = DateTime.Parse(c.DayStart.ToString()),
						DayEnd = DateTime.Parse(c.DayEnd.ToString()),
						Calendars = getCalendar(Convert.ToInt32(c.ID.ToString().Trim())),
						Students = StudentCourse(Convert.ToInt32(c.ID.ToString().Trim())),
					};
                    lisst.Add(course);

                }
			}
			return lisst;

        }
		public List<Course> CoursebyIDLecture()
		{
			List<Course> list = new List<Course> ();
			List<Course> listss = new List<Course>();
			string query = "DECLARE @CurrentDate DATETIME = GETDATE();\r\nSELECT *FROM Course WHERE @CurrentDate BETWEEN DayStart AND DayEnd ";
			DataTable tb = ConnectionData.ExecuteQuery(query);
			foreach(DataRow row in tb.Rows)
			{

                Course obj = new Course()
                {
                    ID = Convert.ToInt32(row["ID"].ToString().Trim()),
                    Title = row["Title"].ToString().Trim(),
                    Detail = row["Detail"].ToString().Trim(),
                    Price = float.Parse(row["Price"].ToString().Trim()),
                    Sale = float.Parse(row["Sale"].ToString().Trim()),
                    Status = bool.Parse(row["Status"].ToString().Trim()),
                    Quantity = int.Parse(row["Quantity"].ToString().Trim()),
                    Image = row["Image"].ToString().Trim(),
                    DayStart = DateTime.Parse(row["DayStart"].ToString().Trim()),
                    DayEnd = DateTime.Parse(row["DayEnd"].ToString().Trim()),
                    Calendars = getCalendar(Convert.ToInt32(row["ID"].ToString().Trim())),
                    Students = StudentCourse(Convert.ToInt32(row["ID"].ToString().Trim())),
					IDLecturer = Convert.ToInt32(row["IDLecturer"].ToString().Trim())

                };
                list.Add(obj);
            }



            return list;
		}
		public List<Course> getCoursebyIDStudent(int id)
		{
			List<Course> list = new List<Course>();
			string query = "select Course.* from Course join CourseOfStudent on Course.ID = CourseOfStudent.IDCourse\r\nwhere CourseOfStudent.IDStudent = @id \r\nand GETDATE() between Course.DayStart and Course.DayEnd ";
			DataTable tb = ConnectionData.ExecuteQuery(query,new object[] { id });
            foreach (DataRow row in tb.Rows)
            {

                Course obj = new Course()
                {
                    ID = Convert.ToInt32(row["ID"].ToString().Trim()),
                    Title = row["Title"].ToString().Trim(),
                    Detail = row["Detail"].ToString().Trim(),
                    Price = float.Parse(row["Price"].ToString().Trim()),
                    Sale = float.Parse(row["Sale"].ToString().Trim()),
                    Status = bool.Parse(row["Status"].ToString().Trim()),
                    Quantity = int.Parse(row["Quantity"].ToString().Trim()),
                    Image = row["Image"].ToString().Trim(),
                    DayStart = DateTime.Parse(row["DayStart"].ToString().Trim()),
                    DayEnd = DateTime.Parse(row["DayEnd"].ToString().Trim()),
                    Calendars = getCalendar(Convert.ToInt32(row["ID"].ToString().Trim())),
                    Students = StudentCourse(Convert.ToInt32(row["ID"].ToString().Trim())),
                    IDLecturer = Convert.ToInt32(row["IDLecturer"].ToString().Trim())

                };
                list.Add(obj);
            }
			return list;
        }

            public List<CommentOfStudent> getComment(int id)
		{
			List<CommentOfStudent> comments = new List<CommentOfStudent>();
			string query = "select * from CommentOfStudent join Student on CommentOfStudent.IDStudent = Student.ID  and CommentOfStudent.IDCourse = @id join Account on Student.IDAccount = Account.IDAccount ";
			DataTable dataTable = ConnectionData.ExecuteQuery(query, new object[] { id });
			foreach (DataRow row in dataTable.Rows)
			{
				CommentOfStudent obj = new CommentOfStudent()
				{
					StudentName = $"{row["FirstName"].ToString().Trim()} {row["LastName"].ToString().Trim()}",
					IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim()),
					Comment = row["Comment"].ToString().Trim(),
					createdDate = DateTime.Parse(row["TimeDate"].ToString().Trim())
				};

				comments.Add(obj);
			}
			comments.Reverse();
			comments.Reverse();
			if (comments.Count > 10)
			{
				return comments.GetRange(0, 10);
			}
			return comments;
		}
		public List<Student> StudentCourse(int id)
		{
			List<Student> StudentCourse = new List<Student>();
			string query = "SELECT * FROM CourseOfStudent, Student join Account on Student.IDAccount = Account.IDAccount WHERE Student.ID = CourseOfStudent.IDStudent AND IDCourse = @id";
			DataTable dataTable = ConnectionData.ExecuteQuery(query, new object[] { id });
			foreach (DataRow row in dataTable.Rows)
			{
				Student obj = new Student()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					FirstName = row["FirstName"].ToString().Trim(),
					LastName = row["LastName"].ToString().Trim(),
					RoleID = Convert.ToInt32(row["RoleID"].ToString().Trim()),
					Password = row["Password"].ToString().Trim(),
					Email = row["Email"].ToString().Trim(),
					Phone = row["Phone"].ToString().Trim(),
					Avatar = row["Avatar"].ToString().Trim(),
					Gender = row["Gender"].ToString().Trim(),
					DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
					ConfirmEmail = Boolean.Parse(row["ConfirmEmail"].ToString().Trim()),
					DateCreate = DateTime.Parse(row["DateCreate"].ToString().Trim())			
				};
				StudentCourse.Add(obj);
			}
			return StudentCourse;
		}
		public DayOfWeek ConvertToDayOfWeek(int dayOfWeekString)
		{
			switch (dayOfWeekString) // Chuyển đổi chuỗi thành chữ thường và kiểm tra
			{
				case 7:
					return DayOfWeek.Sunday;
				case 1:
					return DayOfWeek.Monday;
				case 2:
					return DayOfWeek.Tuesday;
				case 3:
					return DayOfWeek.Wednesday;
				case 4:
					return DayOfWeek.Thursday;
				case 5:
					return DayOfWeek.Friday;
				case 6:
					return DayOfWeek.Saturday;
				default:
					throw new ArgumentException("Invalid day of the week.");
			}
		}
		
		public List<Calendar> getCalendar(int id)
		{
			List<Calendar> calendar = new List<Calendar>();
			string query = "select * from Calendar where IDCourse = @id ";
			DataTable dataTable = ConnectionData.ExecuteQuery(query, new object[] { id });
			foreach (DataRow row in dataTable.Rows)
			{
				Calendar obj = new Calendar()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim()),
					TimeStart = TimeSpan.Parse(row["TimeStart"].ToString().Trim()),
					TimeEnd = TimeSpan.Parse(row["TimeEnd"].ToString().Trim()),
					DayOfWeek = ConvertToDayOfWeek(Convert.ToInt32(row["DayOfWeek"].ToString().Trim()))//1 monday ,2  tuesday
				};

				calendar.Add(obj);
			}
			return calendar;
		}

		public bool Comment(int IDStudent, int IDCourse, string comment)
		{
			DateTime vietnamTime = CurrentDateTime.GetcurrentDateTime;
			string query = "INSERT INTO CommentOfStudent VALUES ( @IDStudent , @IDCourse , @Comment , @TimeDate ) ";
			return ConnectionData.ExecuteUpdate(query, new object[] { IDStudent, IDCourse, comment, vietnamTime.ToString("yyyy-MM-dd HH:mm:ss") });
		}
		public bool AddNew(Course course)
		{
			bool result = false;
			string query = "INSERT INTO Course VALUES " +
				"( @idLecturer , @title , @detail , @price , @sale , 'True', 0) ";
			result = ConnectionData.ExecuteUpdate(query, new object[] { course.IDLecturer, course.Title, course.Detail, course.Price, course.Sale });
			return result;
		}
		public bool Update(Course course)
		{
			bool result = false;
			string query = "UPDATE Course SET " +
				" IDLecturer = @idLecturer , Title = @title , Detail = @detail , Price = @price , Sale = @sale WHERE IDCourse = @id";
			result = ConnectionData.ExecuteUpdate(query, new object[] { course.IDLecturer, course.Title, course.Detail, course.Price, course.Sale, course.ID });
			return result;
		}
		public bool setStatus(Course course)
		{
			bool result = false;
			string query = "UPDATE tblCourse SET " +
				" Status = 1 WHERE IDCourse = @id ";
			if (course.Status)
			{
				query = "UPDATE tblCourse SET " +
				" Status = 0 WHERE IDCourse = @id ";
			}
			result = ConnectionData.ExecuteUpdate(query, new object[] { course.ID });
			return result;
		}
		public bool BuyCourse(int idCourse, int idStudent)
		{
			bool result = false;
			string query = "INSERT INTO tblCourseOfStudent VALUES " +
				" ( @IDstudetn , @Course , @date ) ";
			DateTime vietnamTime = CurrentDateTime.GetcurrentDateTime;
			result = ConnectionData.ExecuteUpdate(query, new object[] { idStudent, idCourse, vietnamTime.ToString("yyyy-MM-dd HH:mm:ss") });
			return result;
		}
		public void SaveChange()
		{
			_coureses.Clear();
			GetAll();
		}

		public bool Delete(int obj)
		{
			throw new NotImplementedException();
		}
	}
}
