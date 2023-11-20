using System.Globalization;

namespace JOT23_Pre2UniOnline.Models
{
	public class Course
	{
		public int ID { get; set; }
		public int IDLecturer { get; set; }
		public Lecturer Lecturer { get; set; }
		public string Title { get; set; }
		public string Detail { get; set; }
		public float Price { get; set; }
		public float Sale { get; set; }
		public bool Status { get; set; }
		public int Quantity { get; set; }
		public int IDSubject { get;set; }
		public string Image { get; set; }
		public DateTime DayStart { get; set; }	
		public DateTime DayEnd { get; set; } 
		public List<Student> Students { get; set; }
		public List<CommentOfStudent> Comments { get; set; }
		public List<Calendar> Calendars { get; set; }
	}
}
