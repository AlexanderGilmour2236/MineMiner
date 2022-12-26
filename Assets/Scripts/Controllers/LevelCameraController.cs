using UnityEngine;
using UnityEngine.EventSystems;

namespace MineMiner
{
    public class LevelCameraController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private float _mouseSensitivity = 0.3f;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _cameraPivot;
        [SerializeField] private Vector3 _cameraStartOffset;
        [SerializeField] private float _cameraMoveSpeed = 0.1f;

        private ICameraMovementStrategy _aroundCameraMovementStrategy;

        private Vector3 _mouseDownPosition = Vector3.zero;

        private Vector2 _mouseDragPosition = Vector2.zero;

        private Vector3 _cameraPivotTargetPosition;

        private IGetLevelRotationStrategy _getLevelRotationStrategy;

        public void Init()
        {
            _aroundCameraMovementStrategy = new AroundCameraMovementStrategy(_camera, _cameraPivot, _cameraStartOffset);
        }

        public void SetGetLevelRotationStrategy(IGetLevelRotationStrategy getLevelRotationStrategy)
        {
            _getLevelRotationStrategy = getLevelRotationStrategy;

        }

        public void Tick()
        {
            Vector3 rotation = _getLevelRotationStrategy.GetRotation(this);
            _aroundCameraMovementStrategy.Rotate(rotation);

            _cameraPivot.position = Vector3.Lerp(_cameraPivot.position, _cameraPivotTargetPosition, _cameraMoveSpeed);
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

        public void setPivotPosition(Vector3 position, bool instantly = false)
        {
            _cameraPivotTargetPosition = position;
            if (instantly)
            {
                _cameraPivot.position = _cameraPivotTargetPosition;
            }
        }

        public Camera Camera
        {
            get { return _camera; }
        }

        public Vector3 MouseDownPosition
        {
            get { return _mouseDownPosition; }
        }

        public Vector2 MouseDragPosition
        {
            get { return _mouseDragPosition; }
        }
        
        public float MouseSensitivity
        {
            get { return _mouseSensitivity; }
        }
    }
}