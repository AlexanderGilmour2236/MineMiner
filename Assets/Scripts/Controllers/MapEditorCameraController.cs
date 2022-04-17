using System;
using UnityEngine;

namespace MineMiner
{
    public class MapEditorCameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _cameraLerpSpeed;
        [SerializeField] private Transform _cameraTargetPoint;
        [SerializeField] private float _moveSpeed;

        private ICameraMovementStrategy _freeCameraMovementStrategy;

        public void Init()
        {
            _freeCameraMovementStrategy = new FreeCameraMovementStrategy(_camera, _cameraLerpSpeed, _cameraTargetPoint);
        }

        public void Tick()
        {
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