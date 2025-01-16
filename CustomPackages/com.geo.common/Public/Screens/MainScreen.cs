using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.Common.Public.Screens
{
    public sealed class MainScreen : ScreenBase
    {
        [SerializeField]
        private Button _rollButton;

        [SerializeField]
        private TextMeshProUGUI _coinsText;

        private int _coins;
        private bool _initialized = false;

        public void Initialize(int coins, UnityAction onRollClick)
        {
            if (_initialized)
            {
                Debug.LogWarning($"The {nameof(MainScreen)} already initialized");
                return;
            }
            _initialized = true;
            _coins = coins;
            _coinsText.text = coins.ToString();

            _rollButton.onClick.AddListener(onRollClick);
        }

        public void DisableRollButton()
        {
            _rollButton.interactable = false;
        }

        public void EnableRollButton()
        {
            _rollButton.interactable = true;
        }

        public void SetCoins(int value)
        {
            StartCoroutine(AnimateCoinsCorroutine(_coins, value));
        }

        private IEnumerator AnimateCoinsCorroutine(int from, int to)
        {
            const float shortTweenDuration = 0.03f;
            var delta = to - from;
            while (delta > 1)
            {
                int change = Mathf.Max(1,  (int) (delta * 0.3f));
                from += change;
                delta -= change;
                SetCoinsWithAnimation(from, shortTweenDuration);
                yield return new WaitForSeconds(shortTweenDuration);
            }

            SetCoinsWithAnimation(to, shortTweenDuration);
        }

        private void SetCoinsWithAnimation(int value, float duration)
        {
            _coinsText.text = value.ToString();
            float scale = 0.0f;
            DOTween.To(() => scale, v =>
            {
                scale = v;
                _coinsText.transform.localScale = Vector3.one * Mathf.Lerp(1.3f, 1.0f, Mathf.Abs(v - 0.5f) * 2);
            }, 1.0f, duration);
        }
    }
}
