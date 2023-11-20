namespace JOT23_Pre2UniOnline.Models
{
	public class Account
	{
		public int IDAccount { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int RoleID { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Gender { get; set; }
		public DateTime DOB { get; set; }
		public DateTime DateCreate { get; set; }
		public string Avatar { get; set; }
		public bool Status { get; set; }
		  public Lecturer Lecturer { get; set; }
		public Student Student { get; set; }
	}

}
