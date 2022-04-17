using System;
using UnityEngine;

namespace MineMiner
{
    public class FreeCameraMovementStrategy : ICameraMovementStrategy
    {
        private Camera _camera;
        private float _cameraLerpSpeed;
        
        private Transform _targetPoint;

        public FreeCameraMovementStrategy(Camera camera, float cameraLerpSpeed, Transform cameraTargetPoint)
        {
            _camera = camera;
            _cameraLerpSpeed = cameraLerpSpeed;
            _targetPoint = cameraTargetPoint;
        }

        public void SetTargetPoint(Transform point)
        {
            _targetPoint = point;
        }

        public void Rotate(Vector3 rotation)
        {
            _targetPoint.Rotate(rotation);
        }

        public void RotateQuaternion(Quaternion rotation)
        {
            _targetPoint.Rotate(rotation.eulerAngles);
        }

        public void Tick()
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, _targetPoint.transform.position, _cameraLerpSpeed * Time.deltaTime);
        }

        public Camera Camera => _camera;
    }
}