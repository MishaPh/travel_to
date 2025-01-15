using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Geo.Common.Public.Screens
{
    public sealed class TextQuizItem : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private Image _buttonBg;
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Sprite _normalSprite;
        [SerializeField]
        private Sprite _failSprite;
        [SerializeField]
        private Sprite _succedSprite;

        private Sequence _sequence;

        public Action OnClick;

        private void Awake()
        {
            _button.onClick.AddListener(ButtonOnClick);
            Clear();
        }

        private void OnEnable()
        {
            transform.localScale = Vector3.zero;
            transform
                .DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBounce);

            _sequence = DOTween.Sequence().SetLoops(-1);

            _sequence.Append(transform
                .DOPunchScale(Vector3.one * 0.02f, 1.0f, 6, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetDelay(UnityEngine.Random.Range(1.0f, 2.0f)));

            _sequence.Join(transform
               .DOShakeRotation(UnityEngine.Random.Range(0.75f, 1.0f), 10.0f, 10, 15f, true)
               .SetEase(Ease.InOutSine)
               .SetDelay(UnityEngine.Random.Range(1.8f, 2.0f)));
        }   

        private void OnDisable()
        {
            _sequence?.Kill();
        }

        public void Show(string text)
        {
            Clear();
            _text.text = text;
        }

        public void Clear()
        {
            _buttonBg.sprite = _normalSprite;
            OnClick = null;
        }

        public void ShowFail()
        {
            _buttonBg.sprite = _failSprite;
        }

        public void ShowSucced()
        {
            _buttonBg.sprite = _succedSprite;
        }

        private void ButtonOnClick()
        {
            OnClick?.Invoke();
        }
    }
}
