using Misc;
using UnityEngine;

namespace MineMiner
{
    public class AroundCameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _cameraPivot;
        [SerializeField] private Vector3 _cameraStartOffset;
        
        private Transform _lookTargetPoint;
        
        private void Start()
        {
            _camera.transform.SetParent(_cameraPivot, true);
        }

        public void SetTargetPoint(Transform point)
        {
            _lookTargetPoint = point;
        }

        private void Update()
        {
            if (_lookTargetPoint != null)
            {
                _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, Quaternion.LookRotation(_lookTargetPoint.position - _camera.transform.position), 0.05f );
            }
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
        
        public void RotateQuaternion(Quaternion rotation)
        {
            _cameraPivot.transform.rotation = Quaternion.identity * rotation;
            
            Vector3 angles = _cameraPivot.transform.localEulerAngles;
            angles.x = angles.x.ClampAngle(-40, 40);
            _cameraPivot.transform.localEulerAngles = angles;
        }
    }
}