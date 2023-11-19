using JOT23_Pre2UniOnline.DataAccess;
using JOT23_Pre2UniOnline.Models;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess.Admin
{
    public class CourseOfStudentDAO : IServiceDAO<CourseOfStudent, CourseOfStudentDAO>
    {
        private static CourseOfStudentDAO _instance;
        private static List<CourseOfStudent> _notification = new List<CourseOfStudent>();
        public static CourseOfStudentDAO Instanse
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CourseOfStudentDAO();
                }
                return _instance;
            }
        }
        public List<CourseOfStudent> CourseOfStudents()
        {

            if (_notification.Count == 0)
            {
                GetAll();
            }
            return _notification;

        }
        public void GetAll()
        {
            _notification.Clear();
            string query = "SELECT * FROM CourseOfStudent";
            DataTable tb = ConnectionData.ExecuteQuery(query);
            foreach (DataRow row in tb.Rows)
            {
                CourseOfStudent obj = new CourseOfStudent()
                {
                    IDStudent = Convert.ToInt32(row["IDStudent"].ToString().Trim()),
                    IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim()),
                    PurchaseDate = DateTime.Parse(row["PurchaseDate"].ToString().Trim()),
                };
                _notification.Add(obj);
            }
            _notification.Reverse();
        }
        public bool AddNew(CourseOfStudent obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int obj)
        {
            throw new NotImplementedException();
        }

      

        public void SaveChange()
        {
            throw new NotImplementedException();
        }

        public bool Update(CourseOfStudent obj)
        {
            throw new NotImplementedException();
        }
    }
}
