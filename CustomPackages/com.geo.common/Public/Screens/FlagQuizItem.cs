using System;
using UnityEngine;
using UnityEngine.UI;

namespace Geo.Common.Public.Screens
{
    public sealed class FlagQuizItem : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Image _borderFail;

        [SerializeField]
        private Image _borderSucced;

        public Action OnClick;

        private void Awake()
        {
            _button.onClick.AddListener(ButtonOnClick);
            Clear();
        }

        private void ButtonOnClick()
        {
            OnClick?.Invoke();
        }

        public void Show(Sprite flag)
        {
            Clear();
            _icon.sprite = flag;
        }

        public void Clear()
        {
            _icon.sprite = null;
            _borderFail.enabled = false;
            _borderSucced.enabled = false;
            OnClick = null;
        }

        public void ShowFail()
        {
            _borderFail.enabled = true;
        }

        public void ShowSucced()
        {
            _borderSucced.enabled = true;
        }
    }
}
