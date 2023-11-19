namespace JOT23_Pre2UniOnline.Models
{
	public class Student : Account
	{
		public int ID { get; set; }
		public int IDAccount { get; set; }
		public bool ConfirmEmail { get; set; }
		public Account Account { get; set; }
		public List<Course> Courses { get; set; }
		public List<CommentOfStudent> Comments { get; set; }
		
	}
	}

