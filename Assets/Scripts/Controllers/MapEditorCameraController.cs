using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MineMiner
{
    public class MapEditorCameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _cameraLerpSpeed;
        [SerializeField] private Transform _cameraTargetPoint;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed = 3;
        [SerializeField] private int _yRotationLimit = 88;

        private ICameraMovementStrategy _freeCameraMovementStrategy;
        private Vector3 _rotation = Vector3.zero;

        public void Init()
        {
            _freeCameraMovementStrategy = new FreeCameraMovementStrategy(_camera, _cameraLerpSpeed, _cameraTargetPoint);
            Cursor.lockState = CursorLockMode.None;
        }

        public void Tick()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                _rotation.x += Input.GetAxis("Mouse X") * _rotationSpeed;
                _rotation.y += Input.GetAxis("Mouse Y") * _rotationSpeed; 

                _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit, _yRotationLimit);

                var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
                var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);

                _freeCameraMovementStrategy.Rotate( xQuat * yQuat);
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                _cameraTargetPoint.Translate(Vector3.forward * _moveSpeed, Space.Self);
            }
            if (Input.GetKey(KeyCode.A))
            {
                _cameraTargetPoint.Translate(Vector3.left * _moveSpeed, Space.Self);
            }
            if (Input.GetKey(KeyCode.D))
            {
                _cameraTargetPoint.Translate(Vector3.right * _moveSpeed, Space.Self);
            }
            if (Input.GetKey(KeyCode.S))
            {
                _cameraTargetPoint.Translate(Vector3.back * _moveSpeed, Space.Self);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _cameraTargetPoint.Translate(Vector3.down * _moveSpeed, Space.Self);
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                _cameraTargetPoint.Translate(Vector3.up * _moveSpeed, Space.Self);
            }


            _freeCameraMovementStrategy.Tick();
        }
    }
}