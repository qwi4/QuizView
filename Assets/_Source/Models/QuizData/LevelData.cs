using System;
using System.Collections.Generic;

namespace Quiz.Models
{
    [Serializable]
    public class LevelData
    {
        public int levelNumber;
        public List<QuestionData> questions;
    }
}