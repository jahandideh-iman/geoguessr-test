using System;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class FollowCamera : MonoBehaviour
    {
        private Vector3 _offset;
        public float followSpeed = 5f;

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

            // Compute the desired position
            Vector3 desiredPosition = _target.position + Quaternion.Euler(0, _target.eulerAngles.y, 0) * _offset;

            // Smoothly move the camera to the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        }
    }
}