﻿namespace JOT23_Pre2UniOnline.Models
{
	public class Quiz
	{
		public int IDQuiz { get; set; }
		public string Question { get; set; }
		public string AnswerA { get; set; }
		public string AnswerB { get; set; }
		public string AnswerC { get; set; }
		public string AnswerD { get; set; }
		public string CorrectAnswer { get; set; }
		public int IDCourse { get; set; }
	}
}
