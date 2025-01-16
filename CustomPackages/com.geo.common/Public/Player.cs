using DG.Tweening;
using Geo.Common.Internal;
using System.Threading.Tasks;
using UnityEngine;

namespace Geo.Common.Public
{
    public sealed class Player : MonoBehaviour, IPlayer
    {
        [SerializeField]
        private Animation _animation;
        [SerializeField]
        private ParticleSystem _hitPs;

        public float JumpPower = 0.5f;

        private void PlayJumpAnimation()
        {
            _animation.Play();
        }

        private void PlayHit()
        {
            _hitPs.Play();
        }

        public Task MoveToAsync(Vector3 position, float duration = 0.3f)
        {
            PlayJumpAnimation();

            return transform
                .DOLocalJump(position, JumpPower, 1, duration)
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

        public void TeleportTo(Vector3 position)
        {
            transform.position = position;
        }
    }
}
