namespace JOT23_Pre2UniOnline.Models
{
    public class ExamTest
    {
        public int ID { get; set; }
        public string NameTest { get; set; }
        public int IDCourse { get; set; }
        public int NumQuestion { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public DateTime DateExam { get; set; }
        public string Password { get; set; }
    }
}
