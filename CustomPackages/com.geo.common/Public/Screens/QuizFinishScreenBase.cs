using Geo.Common.Internal.Quizzes;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Geo.Common.Public.Screens
{
    public abstract class QuizFinishScreenBase : ScreenBase, IPointerClickHandler
    {
        [SerializeField]
        protected TextMeshProUGUI _answerText;

        [SerializeField]
        protected TextMeshProUGUI _rewardText;

        [SerializeField]
        protected GameObject _winText;

        [SerializeField]
        protected GameObject _loseText;

        public bool Completed { get; protected set; }
        public abstract Task ShowResultAsync(QuizGameResult quizResult, CancellationToken token);

        protected async Task WaitForClickAsync(CancellationToken token)
        {
            await Task.Delay(500, token);

            Completed = false;

            while (!Completed && !token.IsCancellationRequested)
            {
                await Task.Delay(100, token);
            }

            Completed = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Completed = true;
        }
    }
}
