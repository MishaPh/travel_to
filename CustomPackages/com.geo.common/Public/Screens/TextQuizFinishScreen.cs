using System.Threading;
using System.Threading.Tasks;

namespace Geo.Common.Public.Screens
{
    public sealed class TextQuizFinishScreen : QuizFinishScreenBase
    {
        public override async Task ShowResultAsync(QuizResult quizResult, CancellationToken token)
        {
            var answer = quizResult.QuizData.Answers[quizResult.QuizData.CorrectAnswerIndex];

            Completed = false;

            _winText.SetActive(quizResult.Win);
            _loseText.SetActive(!quizResult.Win);
            _answerText.text = answer.Text;

            await WaitForClickAsync(token);
        }
    }
}
