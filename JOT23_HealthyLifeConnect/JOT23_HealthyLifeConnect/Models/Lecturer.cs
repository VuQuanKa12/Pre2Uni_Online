namespace JOT23_Pre2UniOnline.Models
{
	public class Lecturer : Account
	{
		public int ID { get; set; }
		public int IDAccount { get; set; }	
		public string Major { get; set; }
		public string Address { get; set; }
		public List<Quiz> Quizs { get; set; }
		public List<Course> Courses { get; set; }
		 public Account Account { get; set; }

		
	}
}
