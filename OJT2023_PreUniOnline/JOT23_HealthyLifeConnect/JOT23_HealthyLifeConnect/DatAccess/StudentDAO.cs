using JOT23_HealthyLifeConnect.Services;
using JOT23_Pre2UniOnline.Models;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess
{
	public class StudentDAO : IUserDAO<StudentDAO>
	{
		private static StudentDAO instance;
		public static StudentDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new StudentDAO();
				}
				return instance;
			}
		}
		private List<Student> _student = new List<Student>();
		public List<Student> Student()
		{
			if (_student.Count == 0)
			{
				GetAll();
			}
			return _student;
		}
		public bool Login(AccountLogin acc)
		{
			string query = "select Student.*, Account.* from Student join Account on Student.IDAccount = Account.IDAccount and Account.Email = @Email and Account.Password = @Password and Account.Status = 1 ";
			DataTable tb = ConnectionData.ExecuteQuery(query, new object[] { acc.Email.Trim(), acc.Password.Trim() });
			foreach (DataRow row in tb.Rows)
			{
				UserLogin obj = new UserLogin()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					IDAccount = Convert.ToInt32(row["IDAccount"].ToString().Trim()),
					FirstName = row["FirstName"].ToString().Trim(),
					LastName = row["LastName"].ToString().Trim(),
					RoleID = Convert.ToInt32(row["RoleID"].ToString().Trim()),
					Password = row["Password"].ToString().Trim(),
					Email = row["Email"].ToString().Trim(),
					Phone = row["Phone"].ToString().Trim(),
					Gender = row["Gender"].ToString().Trim(),
					DOB = DateTime.Parse(row["DOB"].ToString().Trim()),

					ConfirmEmail = Boolean.Parse(row["ConfirmEmail"].ToString().Trim()),
					DateCreate = DateTime.Parse(row["DateCreate"].ToString().Trim()),
					Avatar = row["Avatar"].ToString().Trim(),
					Status = Boolean.Parse(row["Status"].ToString().Trim()),
					Courses = GetsCourses(Convert.ToInt32(row["ID"].ToString().Trim()))

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
		public void GetAll()
		{
			_student.Clear();
			string query = "select * from Student join Account on Student.IDAccount = Account.IDAccount and Account.Status = 1";
			DataTable tb = ConnectionData.ExecuteQuery(query);
			foreach (DataRow row in tb.Rows)
			{
				Student obj = new Student()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					IDAccount = Convert.ToInt32(row["IDAccount"].ToString().Trim()),
					FirstName = row["FirstName"].ToString().Trim(),
					LastName = row["LastName"].ToString().Trim(),
					RoleID = Convert.ToInt32(row["RoleID"].ToString().Trim()),
					Password = row["Password"].ToString().Trim(),
					Email = row["Email"].ToString().Trim(),
					Phone = row["Phone"].ToString().Trim(),
					Gender = row["Gender"].ToString().Trim(),
					DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
					ConfirmEmail = Boolean.Parse(row["ConfirmEmail"].ToString().Trim()),
					DateCreate = DateTime.Parse(row["DateCreate"].ToString().Trim()),
					Avatar = row["Avatar"].ToString().Trim(),
					Status =Boolean.Parse(row["Status"].ToString().Trim()),
					Courses = GetsCourses(Convert.ToInt32(row["ID"].ToString().Trim()))

				};
				_student.Add(obj);
			}
			_student.Reverse();
		}
		public void SaveChange()
		{
			_student.Clear();
			GetAll();
		}

		public List<Student> AdminGetList()
		{
			List<Student> list = new  List<Student>();
			string query = "select * from Student join Account on Student.IDAccount = Account.IDAccount";
			DataTable tb = ConnectionData.ExecuteQuery(query);
            foreach (DataRow row in tb.Rows)
            {
                Student obj = new Student()
                {
                    ID = Convert.ToInt32(row["ID"].ToString().Trim()),
                    IDAccount = Convert.ToInt32(row["IDAccount"].ToString().Trim()),
                    FirstName = row["FirstName"].ToString().Trim(),
                    LastName = row["LastName"].ToString().Trim(),
                    RoleID = Convert.ToInt32(row["RoleID"].ToString().Trim()),
                    Password = row["Password"].ToString().Trim(),
                    Email = row["Email"].ToString().Trim(),
                    Phone = row["Phone"].ToString().Trim(),
                    Gender = row["Gender"].ToString().Trim(),
                    DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
                    ConfirmEmail = Boolean.Parse(row["ConfirmEmail"].ToString().Trim()),
                    DateCreate = DateTime.Parse(row["DateCreate"].ToString().Trim()),
                    Avatar = row["Avatar"].ToString().Trim(),
                    Status = Boolean.Parse(row["Status"].ToString().Trim()),
                    Courses = GetsCourses(Convert.ToInt32(row["ID"].ToString().Trim()))

                };
                list.Add(obj);
            }
			return list;

        }
		public Student Read(int id)
		{
			string query = "select * from Student join Account on Student.IDAccount = Account.IDAccount and Student.ID = @id and Account.Status = 1 ";
			DataTable tb = ConnectionData.ExecuteQuery(query, new object[] { id });
			foreach (DataRow row in tb.Rows)
			{
				Student obj = new Student()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					IDAccount =Convert.ToInt32(row["IDAccount"].ToString().Trim()),
					FirstName = row["FirstName"].ToString().Trim(),
					LastName = row["LastName"].ToString().Trim(),
					RoleID = Convert.ToInt32(row["RoleID"].ToString().Trim()),
					Password = row["Password"].ToString().Trim(),
					Email = row["Email"].ToString().Trim(),
					Phone = row["Phone"].ToString().Trim(),
					Gender = row["Gender"].ToString().Trim(),
					DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
					ConfirmEmail = Boolean.Parse(row["ConfirmEmail"].ToString().Trim()),
					DateCreate = DateTime.Parse(row["DateCreate"].ToString().Trim()),
					Avatar = row["Avatar"].ToString().Trim(),
					Status = Boolean.Parse(row["Status"].ToString().Trim()),
					Courses = GetsCourses(Convert.ToInt32(row["ID"].ToString().Trim()))

				};
				if (obj != null)
				{
					return obj;
				}

			}
			return null;
		}

		public List<Course> GetsCourses(int id)
		{
			List<Course> _courseStudents = new List<Course>();
			string query = "select * from Course , CourseOfStudent where Course.ID =  CourseOfStudent.IDCourse and CourseOfStudent.IDStudent = @id";
			DataTable datatable = ConnectionData.ExecuteQuery(query, new object[] { id });
			foreach (DataRow row in datatable.Rows)
			{
				Course obj = new Course()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					IDLecturer = Convert.ToInt32(row["IDLecturer"].ToString().Trim()),
					Title = row["Title"].ToString().Trim(),
					Detail = row["Detail"].ToString().Trim(),
					Price = float.Parse(row["Price"].ToString().Trim()),
					Sale = float.Parse(row["Sale"].ToString().Trim()),
					Status = bool.Parse(row["Status"].ToString().Trim())

				};
				_courseStudents.Add(obj);
			}
			return _courseStudents;
		}
		public bool uploadAvatar(string name, int id)
		{
			string query = "Update Account set Avatar = @Avatar from Account join Student on Account.IDAccount = Student.IDAccount and Student.ID = @id ";
			bool result = ConnectionData.ExecuteUpdate(query, new object[] { name, id });
			return result;
		}
		
		public bool confirmAccountUser(int id)
		{
			bool result = false;
			DateTime vietnametime = CurrentDateTime.GetcurrentDateTime;
			string query = "update Student set ConfirmEmail = 1 from Student where ID = @id ";
			result = ConnectionData.ExecuteUpdate(query, new object[] {id });
			string query2 = "update Account set DateCreate= @DateCreate from Account join Student on Account.IDAccount = Student.IDAccount and Student.ID = @id ";
			result = ConnectionData.ExecuteUpdate(query2, new object[] { vietnametime.ToString("yyyy-MM-dd HH:mm:ss"), id });
			return result;
		}

		public bool Update(Profile pro)
		{
			int ID = UserLogin.Instance.ID;
			string query = "update Account set FirstName = @FirstName , LastName = @lastName , Password = @Password , Email = @Email , Phone = @Phone , Gender = @Gender , DOB= @DOB from Account join Student on Account.IDAccount = Student.IDAccount where Student.ID = @ID";

			bool result = ConnectionData.ExecuteUpdate(query, new object[] { pro.FirstName, pro.LastName, pro.Password, pro.Email, pro.Phone, pro.Gender, pro.DOB.ToString("yyyy-MM-dd"), pro.ID });
			return result;
		}
		public int getIDbyEmail(string email)
		{
			int id =0;
			string query = " select * from Student join Account on Student.IDAccount = Account.IDAccount and Account.Email = @email ";
			DataTable datatable = ConnectionData.ExecuteQuery(query, new object[] { email });
			foreach(DataRow row in datatable.Rows)
			{
				 id = Convert.ToInt32(row["ID"]);
			}
			return id;
		}

		public bool AddNew(Profile pro)
		{
			DateTime vietnamTime = CurrentDateTime.GetcurrentDateTime;
			bool result = false;
			string query = "insert into Account(FirstName,LastName,RoleID,Password,Email,Phone,Gender,DOB,DateCreate,Avatar, Status) values( @FirstName , @LastName , 3 , @Password , @Email , @Phone , @Gender , @DOB , @DateCreate , 'Female.jpg' , 1 )\r\nDECLARE @IDAccountNew INT;\r\nSET @IDAccountNew = SCOPE_IDENTITY();\r\nINSERT into Student Values(@IDAccountNew, 0)";
			result = ConnectionData.ExecuteUpdate(query, new object[] { pro.FirstName, pro.LastName, pro.Password, pro.Email, pro.Phone, pro.Gender, pro.DOB.ToString("yyyy-MM-dd"), vietnamTime.ToString("yyyy-MM-dd HH:mm:ss") });
			return result;
		}
		public bool resetPassword(Profile pro)
		{
			bool result = false;
			string query = "update Account set Email = @Email , Password = @password from Account join Student on Account.IDAccount = Student.IDAccount and Student.ID =  @id ";
			result = ConnectionData.ExecuteUpdate(query, new object[] {  pro.Email , pro.Password , pro.ID });
			return result;
		}
		public bool ChangePass(int Id, string Pass)
		{
			bool result = false;
			string query = "update Account set Password = @Pass from Account join Student on Account.IDAccount = Student.IDAccount and Student.ID = @idStudent ";
			result = ConnectionData.ExecuteUpdate(query, new object[] { Pass, Id });
			return result;
		}

		public bool Delete(int index)
		{
			throw new NotImplementedException();
		}
		public bool Lock(int IDStudent)
		{
			bool result = false;
			string query = "update Account set Status = 0 from Account join Student on Account.IDAccount = Student.IDAccount where Student.ID = @id ";
			result = ConnectionData.ExecuteUpdate (query, new object[] { IDStudent });
			return result;
		}
		public bool Unlock( int IDStudent)
		{
			bool result = false;
            string query = "update Account set Status = 1 from Account join Student on Account.IDAccount = Student.IDAccount where Student.ID = @id ";
            result = ConnectionData.ExecuteUpdate(query, new object[] { IDStudent });
            return result;
        }
		public List<Student> AdminReadListStudent()
		{
			List<Student> list = new List<Student>();
			string query = "select * from Student join Account on Student.IDAccount = Account.IDAccount";
			DataTable tb = ConnectionData.ExecuteQuery(query);
			foreach(DataRow row  in tb.Rows)
			{
				Student obj = new Student()
				{
					ID = Convert.ToInt32(row["ID"].ToString().Trim()),
					IDAccount = Convert.ToInt32(row["IDAccount"].ToString().Trim()),
					FirstName = row["FirstName"].ToString().Trim(),
					LastName = row["LastName"].ToString().Trim(),
					RoleID = Convert.ToInt32(row["RoleID"].ToString().Trim()),
					Password = row["Password"].ToString().Trim(),
					Email = row["Email"].ToString().Trim(),
					Phone = row["Phone"].ToString().Trim(),
					Gender = row["Gender"].ToString().Trim(),
					DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
					ConfirmEmail = Boolean.Parse(row["ConfirmEmail"].ToString().Trim()),
					DateCreate = DateTime.Parse(row["DateCreate"].ToString().Trim()),
					Avatar = row["Avatar"].ToString().Trim(),
					Status = Boolean.Parse(row["Status"].ToString().Trim()),
					Courses = GetsCourses(Convert.ToInt32(row["ID"].ToString().Trim()))

				};
				list.Add(obj);
			}
			return list;
		}
		
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 