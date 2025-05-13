using System;
using System.Collections.Generic;

namespace Quiz.Models
{
    [Serializable]
    public class QuestionData
    {
        public string type;
        public string text;
        public List<string> options;
        public int correctOptionIndex;
        public bool correctAnswer;
    }
}