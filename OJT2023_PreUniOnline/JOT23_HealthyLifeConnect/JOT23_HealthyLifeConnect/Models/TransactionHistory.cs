namespace JOT23_Pre2UniOnline.Models
{
	public class TransactionHistory
	{
		public int ID { get; set; }
		public int IDStudent { get; set; }
		public string CourseTitle { get; set; }
		public double Price { get; set; }
		public DateTime Date { get; set; }
		public string Method { get; set; }
		public string OrderID { get; set; }
	}
}
