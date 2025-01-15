using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
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

        public event Action OnRoll;

        private void Awake()
        {
            _rollButton.onClick.AddListener(() => { OnRoll?.Invoke(); });
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
            _coins = value;
            _coinsText.text = value.ToString();
        }

        public void AnimateCoinsTo(int to)
        {
            StartCoroutine(AnimateCoinsCorroutine(_coins, to));
        }

        private IEnumerator AnimateCoinsCorroutine(int from, int to)
        {
            var delta = to - from;
            while (delta > 1)
            {
                int change = Mathf.Max(1,  (int) (delta * 0.3f));
                from += change;
                delta -= change;
                SetCoinsWithAnimation(from);
                yield return new WaitForSeconds(0.04f);
            }

            SetCoinsWithAnimation(to);
        }

        private void SetCoinsWithAnimation(int value)
        {
            _coinsText.text = value.ToString();
            float scale = 0.0f;
            DOTween.To(() => scale, v =>
            {
                scale = v;
                _coinsText.transform.localScale = Vector3.one * Mathf.Lerp(1.3f, 1.0f, Mathf.Abs(v - 0.5f) * 2);
            }, 1.0f, 0.03f);
        }
    }
}
