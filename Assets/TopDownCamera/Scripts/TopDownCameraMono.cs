using System;
using UnityEngine;

namespace TopDownCamera.Scripts
{
    public class TopDownCameraMono : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _target;
        [SerializeField] private float _height;
        [SerializeField] private float _distance;
        [SerializeField] private float _angle;

        [SerializeField] private float _smoothSpeed;
        
        private Vector3 _refVelocity;
        
        protected void Start()
        {
            HandleCamera();
        }

        protected void Update()
        {
            HandleCamera();
        }

        public virtual void HandleCamera()
        {
            if (_target == null)
            {
                return;
            }

            var worldPosition = (Vector3.forward * _distance) + (Vector3.up * _height);
            var rotateVector = Quaternion.AngleAxis(_angle, Vector3.up) * worldPosition;
            
            var flatTargetPosition = _target.position;
            flatTargetPosition.y = 0f;

            var finalPosition = flatTargetPosition + rotateVector;

            _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, finalPosition, ref _refVelocity, _smoothSpeed);
            _cameraTransform.LookAt(flatTargetPosition);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            if (_target != null)
            {
                Gizmos.DrawLine(_cameraTransform.position, _target.position);
                Gizmos.DrawSphere(_target.position, 1.5f);
            }
            
            Gizmos.DrawSphere(_cameraTransform.position, 1.5f);
        }
    }
}