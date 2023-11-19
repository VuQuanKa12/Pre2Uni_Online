using JOT23_Pre2UniOnline.Models;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess.Admin
{
    public class AdminDAO : IUserDAO<AdminDAO>
    {
        private static AdminDAO instance;

        public static AdminDAO Instence()
        {
            if (instance == null)
            {
                instance = new AdminDAO();
            }
            return instance;
        }

        public bool Login(AccountLogin acc)
        {
            string query = "SELECT Admin.*, Account.* FROM Admin " +
                   "JOIN Account ON Admin.IDAccount = Account.IDAccount " +
                   "WHERE Account.Email = @Email AND Account.Password = @Password and Account.Status = 1";
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
                    Gender = row["Gender"].ToString().Trim(),
                    DOB = DateTime.Parse(row["DOB"].ToString().Trim()),
                    Status = Boolean.Parse(row["Status"].ToString().Trim())

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

        public bool AddNew(Profile obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int index)
        {
            throw new NotImplementedException();
        }

        public void GetAll()
        {
            throw new NotImplementedException();
        }

        public void SaveChange()
        {
            throw new NotImplementedException();
        }

        public bool Update(Profile obj)
        {
            throw new NotImplementedException();
        }
    }
}
