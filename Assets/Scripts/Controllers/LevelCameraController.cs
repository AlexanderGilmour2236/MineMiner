using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MineMiner
{
    public class LevelCameraController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Inject] private BlocksController _blocksController;
        [Inject] private ICameraController _aroundCameraController;
        
        [SerializeField] private float _mouseSensitivity = 0.3f;
        
        private Vector3 _mouseDownPosition = Vector3.zero;
        private Vector2 _mouseDragPosition = Vector2.zero;
        
        public void Tick()
        {
            _blocksController.Tick();

            float xRotation = (_mouseDragPosition.y - _mouseDownPosition.y) * _mouseSensitivity;
            float yRotation = (_mouseDragPosition.x - _mouseDownPosition.x) * _mouseSensitivity;

            _aroundCameraController.Rotate(new Vector3(-xRotation, yRotation));

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