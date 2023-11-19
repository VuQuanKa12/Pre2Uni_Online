namespace JOT23_Pre2UniOnline.Models
{
    public class FAQ
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public static List<FAQ> faqs = new List<FAQ>()
{
  new FAQ
  {
    Question = "Thủ đô Việt Nam là gì?",
    Answer = "Hà Nội"
  },

  new FAQ
  {
    Question = "Ngày nào là ngày Quốc khánh?",
    Answer = "Ngày 2/9"
  }
};
    }

}
