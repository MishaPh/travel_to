using System;

namespace Geo.Common.Internal.Quizzes
{
    public class QuizData
    {
        [Serializable]
        public struct AnswerData
        {
            public string ImageID;
            public string Text;
        }

        public Guid ID;
        public int QuestionType;
        public string Question;
        public string CustomImageID;

        public AnswerData[] Answers;

        public int CorrectAnswerIndex;
    }
}
