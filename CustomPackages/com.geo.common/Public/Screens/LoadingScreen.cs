using DG.Tweening;
using Geo.Common.Internal.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace Geo.Common.Public.Screens
{
    public sealed class LoadingScreen : ScreenBase
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private Tween _tween;

        public void FadeIn(float duration = 0.3f)
        {
            FadeInAsync(duration).Forget();
        }

        public void FadeOut(float duration = 0.3f)
        {
            FadeOutAsync(duration).Forget();
        }

        public Task FadeInAsync(float duration = 0.3f)
        {
            _tween?.Kill();
            gameObject.SetActive(true);
            _tween = _canvasGroup
                .DOFade(1, duration)
                .SetEase(Ease.InOutSine);

            return _tween.AsyncWaitForCompletion();
        }

        public Task FadeOutAsync(float duration = 0.3f)
        {
            _tween?.Kill();
            _tween = _canvasGroup
                .DOFade(0, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            return _tween.AsyncWaitForCompletion();
        }
    }
}
