using Geo.Common.Internal;
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
        private const float DelayBeforeFinishTouch = 0.3f;

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
            await Task.Delay(DelayBeforeFinishTouch.SecondsToTicks(), token);

            Completed = false;

            while (!Completed && !token.IsCancellationRequested)
            {
                await Task.Yield();
            }

            Completed = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Completed = true;
        }
    }
}
