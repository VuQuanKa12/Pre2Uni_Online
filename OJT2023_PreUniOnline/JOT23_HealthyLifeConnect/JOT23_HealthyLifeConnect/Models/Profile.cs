namespace JOT23_Pre2UniOnline.Models
{
	public class Profile
	{
		private static Profile _instance;
		public static Profile Instance
		{
			get
			{
				if (_instance == null)
					_instance = new Profile();
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}
		public int ID { get; set; }
		public int IDAccount { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public DateTime DOB { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int RoleID { get; set; }
	}
}
