using Misc;
using UnityEngine;

namespace MineMiner
{
    public class AroundCameraMovementStrategy : ICameraMovementStrategy
    { 
        private Camera _camera;
        private Transform _cameraPivot;
        private Vector3 _cameraStartOffset;
        
        private Transform _lookTargetPoint;
        
        public AroundCameraMovementStrategy(Camera camera, Transform cameraPivot, Vector3 cameraStartOffset = new Vector3())
        {
            _camera = camera;
            _cameraPivot = cameraPivot;
            _cameraStartOffset = cameraStartOffset;
            
            _camera.transform.SetParent(_cameraPivot, true);
        }

        public void SetTargetPoint(Transform point)
        {
            _lookTargetPoint = point;
        }

        public void Tick()
        { 
            _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, Quaternion.LookRotation(_lookTargetPoint.position - _camera.transform.position), 0.05f );
        }

        public Camera Camera
        {
            get { return _camera; }
        }

        public void Rotate(Vector3 rotation)
        {
            _cameraPivot.transform.eulerAngles += rotation;
            
            Vector3 angles = _cameraPivot.transform.localEulerAngles;
            angles.x = angles.x.ClampAngle(-40, 40);
            _cameraPivot.transform.localEulerAngles = angles;
        }
        
        public void Rotate(Quaternion rotation)
        {
            _cameraPivot.transform.rotation = Quaternion.identity * rotation;
            
            Vector3 angles = _cameraPivot.transform.localEulerAngles;
            angles.x = angles.x.ClampAngle(-40, 40);
            _cameraPivot.transform.localEulerAngles = angles;
        }
    }
}