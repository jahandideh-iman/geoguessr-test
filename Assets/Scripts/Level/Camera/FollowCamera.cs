#nullable enable
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class FollowCamera : MonoBehaviour
    {
        private Vector3 _offset;
        public float _followSpeed = 5f;

        private Transform? _target;

        public void Awake()
        {
            _offset = transform.position;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void ClearTarget()
        {
            _target = null;
        }

        void LateUpdate()
        {
            if (_target == null)
            {
                return;
            }

            Vector3 desiredPosition = _target.position + Quaternion.Euler(0, _target.eulerAngles.y, 0) * _offset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);
        }
    }
}