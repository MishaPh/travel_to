using System.Threading.Tasks;
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

        public void PlayAnimation(Camera camera, int countCoins)
        {
            _canvas.worldCamera = camera;
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
