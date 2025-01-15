using DG.Tweening;
using Geo.Common.Internal;
using System.Threading.Tasks;
using UnityEngine;

namespace Geo.Common.Public
{
    public class Player : MonoBehaviour, IUnit
    {
        [SerializeField]
        private Animation _animation;
        [SerializeField]
        private ParticleSystem _hitPs;

        public int CurrnetIndex { get; private set; }
        public int Score { get; private set; }

        private void PlayJumpAnimation()
        {
            _animation.Play("PlayerJump");
        }

        private void PlayHit()
        {
            _hitPs.Play();
        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public void SetIndex(int value)
        {
            CurrnetIndex = value;
        }

        public Task MoveToAsync(Vector3 position, float duration = 0.3f)
        {
            PlayJumpAnimation();

            return transform
                .DOLocalJump(position, 0.5f, 1, duration)
                .SetEase(Ease.OutSine)
                .OnComplete(PlayHit).AsyncWaitForCompletion();
        }

        public Task PlayFinishMoveAsync()
        {
            return transform
               .DOBlendableRotateBy(new Vector3(0, 180, 0), 0.4f)
               .SetEase(Ease.OutQuad)
               .AsyncWaitForCompletion();
        }
    }
}
