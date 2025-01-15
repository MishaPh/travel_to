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

        public async Task ShowAwait(Camera camera, int count)
        {
            _canvas.worldCamera = camera;
            _rewardText.text = count.ToString();

            _animation.Play();

            while (_animation.isPlaying)
            {
                await Task.Yield();
            }
        }
    }
}
