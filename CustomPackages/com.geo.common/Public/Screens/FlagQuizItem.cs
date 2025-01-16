using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
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

        private Sequence _sequence;

        private void Awake()
        {
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
                .DOPunchScale(Vector3.one * 0.01f, 1.0f, 3, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetDelay(Random.Range(1.5f, 2.0f)));

            _sequence.Append(transform
                .DOPunchScale(Vector3.one * 0.02f, 1.0f, 6, 0.3f)
                .SetEase(Ease.InOutBounce)
                .SetDelay(Random.Range(1.5f, 2.0f)));
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }

        public void Show(Sprite flag, UnityAction onClick)
        {
            Clear();
            _icon.sprite = flag;
            _button.onClick.AddListener(onClick);
            _button.enabled = onClick != null;
        }

        public void Clear()
        {
            _icon.sprite = null;
            _borderFail.enabled = false;
            _borderSucced.enabled = false;
            _button.onClick.RemoveAllListeners();
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
