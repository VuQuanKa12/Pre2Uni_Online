using JOT23_HealthyLifeConnect.Services;
using JOT23_Pre2UniOnline.Models;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess
{
	public class TransactionDAO : IServiceDAO<TransactionHistory, TransactionDAO>
	{
		private static TransactionDAO _instance;
		private static List<TransactionHistory> _transactions = new List<TransactionHistory>();

		public static TransactionDAO Instanse
		{
			get
			{
				if (_instance == null)
				{
					_instance = new TransactionDAO();
				}
				return _instance;
			}
		}
		public List<TransactionHistory> Transactions()
		{
			if (_transactions.Count == 0)
			{
				GetAll();
			}
			return _transactions;

		}

		public void GetAll()
		{
			_transactions.Clear();
			string query = "SELECT * FROM TransactionHistory";
			DataTable tb = ConnectionData.ExecuteQuery(query);
			foreach (DataRow row in tb.Rows)
			{
				TransactionHistory obj = new TransactionHistory()
				{
					ID = Int32.Parse(row["ID"].ToString().Trim()),
					IDStudent = Int32.Parse(row["IDStudent"].ToString().Trim()),
					CourseTitle = row["CourseTitle"].ToString().Trim(),
					Price = Double.Parse(row["Price"].ToString().Trim()),
					Date = DateTime.Parse(row["Date"].ToString().Trim()),
					Method = row["Method"].ToString().Trim(),
					OrderID = row["OrderID"].ToString().Trim(),
				};
				_transactions.Add(obj);
			}
			_transactions.Reverse();
		}

		public bool AddNew(TransactionHistory trans)
		{
			DateTime vietnamTime = CurrentDateTime.GetcurrentDateTime;
			bool result = false;
			string query = "INSERT INTO TransactionHistory " +
				"\nVALUES ( @idStudent , @courseTItle , @price , @date , @method , @idtrans )";
			result = ConnectionData.ExecuteUpdate(query, new object[] { trans.IDStudent, trans.CourseTitle, trans.Price, vietnamTime.ToString("yyyy-MM-dd HH:mm:ss"), trans.Method, trans.OrderID });
			return result;
		}
		public void SaveChange()
		{
			_transactions.Clear();
			GetAll();
		}

		public bool Update(TransactionHistory obj)
		{
			throw new NotImplementedException();
		}

		public bool Delete(int obj)
		{
			throw new NotImplementedException();
		}
	}
}
