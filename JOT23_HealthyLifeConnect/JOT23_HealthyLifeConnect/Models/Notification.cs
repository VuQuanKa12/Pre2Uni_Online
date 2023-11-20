namespace JOT23_Pre2UniOnline.Models
{
	public class Notification
	{
		public int ID { get; set; }
		public int IdAdmin { get;set; }
		public string Title { get; set; }
		public string Detail { get;set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
	}
}
