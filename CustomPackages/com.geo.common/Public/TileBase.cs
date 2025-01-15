using DG.Tweening;
using Geo.Common.Internal.Boards;
using UnityEngine;

namespace Geo.Common.Public
{
    public class TileBase : MonoBehaviour, ITileItem
    {
        [SerializeField]
        private Transform _model;
        [SerializeField]
        private ParticleSystem _psHit;
        [SerializeField]
        private Vector2 _scaleRange = new (0.8f, 1.0f);

        private void Start()
        {
            _model.localScale = Vector3.one * Random.Range(_scaleRange.x, _scaleRange.y);
        }

        public void OnHit()
        {
            _model.DOLocalJump(transform.position, -0.05f, 1, 0.5f).SetEase(Ease.OutBounce);
            _psHit.Play();
        }
    }
}
