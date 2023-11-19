using JOT23_Pre2UniOnline.DatAccess;
using JOT23_Pre2UniOnline.Models;
using JOT23_HealthyLifeConnect.Services;
using System.Data;

namespace JOT23_Pre2UniOnline.DataAccess
{
    public class NotificationDAO : IServiceDAO<Notification, NotificationDAO>
    {
        private static NotificationDAO _instance;
        private static List<Notification> _notification = new List<Notification>();

        public static NotificationDAO Instanse
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NotificationDAO();
                }
                return _instance;
            }
        }
        public List<Notification> Transactions()
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
            string query = "SELECT * FROM Notification";
            DataTable tb = ConnectionData.ExecuteQuery(query);
            foreach (DataRow row in tb.Rows)
            {
                Notification obj = new Notification()
                {
					ID = Convert.ToInt32(row["ID"].ToString().Trim()), // Sửa tên cột từ "IDContification" thành "ID"
					IdAdmin = Convert.ToInt32(row["IdAdmin"].ToString().Trim()),
                    Title = row["Title"].ToString().Trim(),
					Detail = row["Detail"].ToString().Trim(),
					StartTime = DateTime.Parse(row["StartTime"].ToString().Trim()), // Sửa tên cột từ "StartDate" thành "StartTime"
					EndTime = DateTime.Parse(row["EndTime"].ToString().Trim()),
				};
                _notification.Add(obj);
            }
            _notification.Reverse();
        }

        public bool AddNew(Notification notification)
        {
            DateTime vietnamTime = CurrentDateTime.GetcurrentDateTime;
            bool result = false;
            string query = "INSERT INTO Notification (IdAdmin , Title, Detail, StartTime, EndTime) " +
				   "VALUES ( @Idadmin , @title , @detail , @start , @end )";
            result = ConnectionData.ExecuteUpdate(query, new object[] { notification.IdAdmin , notification.Title, notification.Detail, notification.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), notification.EndTime.ToString("yyyy-MM-dd HH:mm:ss") });
            return result;
        }
        public bool Delete(int id)
        {
            bool result = false;
            string query = "DELETE FROM Notification WHERE ID = @id ";
            result = ConnectionData.ExecuteUpdate(query, new object[] { id });
            return result;
        }
        public void SaveChange()
        {
            _notification.Clear();
            GetAll();
        }

        public bool Update(Notification obj)
        {
            throw new NotImplementedException();
        }

       
    }
}
