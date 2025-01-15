using UnityEngine;

namespace Geo.Common.Public
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float smoothSpeed = 0.125f; 
        [SerializeField] private Vector2 threshold;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector2 difference;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            offset = transform.position - player.position;
        }

        private void LateUpdate()
        {
            if (player == null)
                return;

            Vector3 cameraPosition = transform.position;
            Vector3 targetPosition = player.position + offset;

            difference = _camera.WorldToViewportPoint(player.position);
            difference.x = Mathf.Abs(difference.x - 0.5f);
            difference.y = Mathf.Abs(difference.y - 0.5f);

            if (difference.x > threshold.x || difference.y > threshold.y)
            {
                transform.position = Vector3.Lerp(cameraPosition, targetPosition, smoothSpeed);
            }
        }
    }
}
