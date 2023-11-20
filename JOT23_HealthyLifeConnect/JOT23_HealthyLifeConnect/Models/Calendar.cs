namespace JOT23_Pre2UniOnline.Models
{
	public class Calendar
	{
		public int ID { get; set; }
		public int IDCourse { get; set; }
		public string Namecourse { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public TimeSpan TimeStart { get; set; }
		public TimeSpan TimeEnd { get; set; }
	}
}
