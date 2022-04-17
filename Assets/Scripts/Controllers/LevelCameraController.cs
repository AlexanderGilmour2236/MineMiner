using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MineMiner
{
    public class LevelCameraController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private float _mouseSensitivity = 0.3f;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _cameraPivot;
        [SerializeField] private Vector3 _cameraStartOffset;

        private ICameraMovementStrategy _aroundCameraMovementStrategy;
        
        private Vector3 _mouseDownPosition = Vector3.zero;
        private Vector2 _mouseDragPosition = Vector2.zero;

        public void Init()
        {
            _aroundCameraMovementStrategy = new AroundCameraMovementStrategy(_camera, _cameraPivot, _cameraStartOffset);
        }
        
        public void Tick()
        {
            float xRotation = (_mouseDragPosition.y - _mouseDownPosition.y) * _mouseSensitivity;
            float yRotation = (_mouseDragPosition.x - _mouseDownPosition.x) * _mouseSensitivity;

            _aroundCameraMovementStrategy.Rotate(new Vector3(-xRotation, yRotation));

            _mouseDownPosition = Vector3.Lerp(_mouseDownPosition, _mouseDragPosition, 0.05f);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _mouseDownPosition = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            _mouseDragPosition = Input.mousePosition;
        }

        public void StopDrag()
        {
            _mouseDownPosition = Input.mousePosition;
            _mouseDragPosition = _mouseDownPosition;
        }
    }
}