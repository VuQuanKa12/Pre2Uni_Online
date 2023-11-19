namespace JOT23_Pre2UniOnline.Models
{
    public class ResultQuiz
    {
        public int ID { get; set; }
        public int IDExamTest { get; set; }
        public int IDStudent { get; set; }
        public float Point { get; set; }
        public  int NumberAnswerCorrect{get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NameExam { get; set; }
    }
}
