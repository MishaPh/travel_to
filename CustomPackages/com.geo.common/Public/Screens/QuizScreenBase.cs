using DG.Tweening;
using Geo.Common.Internal.Quizzes;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Geo.Common.Public.Screens
{
    public abstract class QuizScreenBase : ScreenBase
    {
        private const int AmountOfSecondsToSolveQuiz = 15; 

        [SerializeField]
        private TextMeshProUGUI _timeText;

        private float _timeToEnd;
        private int _timeToEndInt;
        private bool _isTimerActive = false;

        public abstract Task ShowAsync(QuizData data, Action<QuizGameResult> resultCallback, CancellationToken token);

        protected void StartTimer()
        {
            _isTimerActive = true;
            _timeToEnd = AmountOfSecondsToSolveQuiz;
            _timeToEndInt = AmountOfSecondsToSolveQuiz;
            _timeText.text = string.Format($"{_timeToEndInt}s");
        }

        protected void StopTimer()
        {
            _isTimerActive = false;
        }

        protected abstract void OnTimeOut();

        public void Update()
        {
            if (!_isTimerActive)
                return;

            _timeToEnd -= Time.deltaTime;

            var second = Mathf.FloorToInt(_timeToEnd);
            if (second == _timeToEndInt)
                return;

            _timeToEndInt = second;

            _timeText.text = string.Format($"{_timeToEndInt}s");

            _timeText.transform
                .DOShakeRotation(0.5f, 20.0f - _timeToEndInt, 10 - _timeToEndInt, 35f, true)
                .SetEase(Ease.InOutSine);

            if (_timeToEndInt == 0)
            {
                StopTimer();
                OnTimeOut();
            }
        }
    }
}
