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

        public Action OnClick;

        private void Awake()
        {
            _button.onClick.AddListener(ButtonOnClick);
            Clear();
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
