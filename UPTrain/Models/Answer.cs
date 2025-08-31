﻿namespace UPTrain.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string SelectedAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question? Question { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
