using UnityEngine;

namespace Geo.Common.Public
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] 
        private Transform _target;

        [SerializeField] 
        private float smoothSpeed = 0.125f; 

        [SerializeField] 
        private Vector2 threshold;

        private Vector3 _offset;
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _offset = transform.position - _target.position;
        }

        private void LateUpdate()
        {
            if (_target == null)
                return;

            Vector3 cameraPosition = transform.position;
            Vector3 targetPosition = _target.position + _offset;

            var difference = DistanceToCameraViewCenter(_target.position);

            if (difference.x > threshold.x || difference.y > threshold.y)
            {
                transform.position = Vector3.Lerp(cameraPosition, targetPosition, smoothSpeed);
            }
        }

        private Vector2 DistanceToCameraViewCenter(Vector3 position)
        {
            var difference = _camera.WorldToViewportPoint(position);
            difference.x = Mathf.Abs(difference.x - 0.5f);
            difference.y = Mathf.Abs(difference.y - 0.5f);
            return difference;
        }
    }
}
