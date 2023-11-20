using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace JOT23_Pre2UniOnline.Models
{
	public class CommentOfStudent
	{
		public string StudentName { get; set; }
		public int IDCourse { get;set; }
		public string Comment { get; set; }	
		public DateTime createdDate { get; set; }
	}
}
