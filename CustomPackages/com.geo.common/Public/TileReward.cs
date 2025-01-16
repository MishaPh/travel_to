using TMPro;
using UnityEngine;

namespace Geo.Common.Public
{
    public sealed class TileReward : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private TextMeshProUGUI _rewardText;

        [SerializeField]
        private Animation _animation;

        public float GetAnimationDuratiion() => _animation.clip.length;

        public void PlayAnimation(int countCoins)
        {
            _canvas.worldCamera = Camera.main;
            _rewardText.text = countCoins.ToString();
            _animation.Play();

            Invoke(nameof(SelfDestroy), GetAnimationDuratiion());
        }

        private void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}
