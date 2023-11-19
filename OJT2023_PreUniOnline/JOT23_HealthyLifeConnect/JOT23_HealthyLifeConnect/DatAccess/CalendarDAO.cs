using JOT23_Pre2UniOnline.Models;
using System.Data;

namespace JOT23_Pre2UniOnline.DatAccess
{
	public class CalendarDAO : IServiceDAO<Calendar, CalendarDAO>
	{
		private List<Calendar> _calendar = new List<Calendar>();
		private static CalendarDAO insctance;
		public static CalendarDAO Instance
		{
			get
			{
				if (insctance == null)
				{
					insctance = new CalendarDAO();
				}
				return insctance;
			}
		}

		public List<Calendar> Calendars()
		{
			_calendar.Clear();
			if (_calendar.Count == 0)
			{
				GetAll();
			}
			return _calendar;
		}
		private CalendarDAO() { }
		public bool AddNew(Calendar calendar)
		{
			bool result = false;
			string query = "INSERT INTO Calendar VALUES " +
				" ( @IDCourse , @start , @end ) ";
			result = ConnectionData.ExecuteUpdate(query, new object[] { calendar.IDCourse, calendar.TimeStart, calendar.TimeEnd });
			return result;
		}
		public bool AddNewCalendar(int idcourse, TimeSpan timeStart, TimeSpan timeEnd , int days)
		{
			bool result = false;
			string query = "INSERT INTO Calendar(IDCourse,TimeStart,TimeEnd,DayOfWeek) VALUES " +
				" ( @IDCourse , @start , @end , @day ) ";
			result = ConnectionData.ExecuteUpdate (query, new object[] { idcourse, timeStart, timeEnd, days });
			return result;
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
		public void GetAll()
		{
			_calendar.Clear();
			if (_calendar.Count == 0)
			{
				string query = "SELECT Calendar.*, Course.Title FROM Course, Calendar WHERE Course.ID = Calendar.IDCourse ORDER BY Calendar.TimeStart ASC;";
				DataTable dataTable = ConnectionData.ExecuteQuery(query);
				foreach (DataRow row in dataTable.Rows)
				{

					Calendar obj = new Calendar()
					{
						ID = Convert.ToInt32(row["ID"].ToString().Trim()),
						IDCourse = Convert.ToInt32(row["IDCourse"].ToString().Trim()),
						TimeStart = TimeSpan.Parse(row["TimeStart"].ToString().Trim()),
						TimeEnd = TimeSpan.Parse(row["TimeEnd"].ToString().Trim()),
						Namecourse = row["Title"].ToString().Trim(),
						DayOfWeek = ConvertToDayOfWeek(Convert.ToInt32(row["DayOfWeek"].ToString().Trim()))
					};

					_calendar.Add(obj);

				}
				_calendar = _calendar.OrderBy(calendar => calendar.IDCourse).ToList();
			}
		}

		public void SaveChange()
		{
			_calendar.Clear();
			GetAll();
		}

		public bool Update(Calendar calendar)
		{
			string query = "UPDATE tblCalendar SET TimeStart = @start , TimeEnd = @end WHERE ID = @IDCalendar ";
			return ConnectionData.ExecuteUpdate(query, new object[] { calendar.TimeStart.ToString(@"hh\:mm\:ss"), calendar.TimeEnd.ToString(@"hh\:mm\:ss"), calendar.ID });
		}

		public bool Delete(int index)
		{
			bool result;
			string query = " DELETE FROM Calendar WHERE ID = @index ";
			result =  ConnectionData.ExecuteUpdate(query, new object[] { index });
			return result;
		}
	}
}
