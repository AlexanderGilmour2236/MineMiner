using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MineMiner
{
    public class LevelCameraController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Inject] private BlocksController _blocksController;
        [Inject] private ICameraController _aroundCameraController;
        
        [SerializeField] private float mouseSensitivity = 0.3f;
        
        private bool _pointerDown;
        private Vector3 _mouseDownPosition = Vector3.zero;
        private Vector2 _mouseDragPosition = Vector2.zero;
        
        public void Tick()
        {
            _blocksController.Tick();

            float xRotation = (_mouseDragPosition.y - _mouseDownPosition.y) * mouseSensitivity;
            float yRotation = (_mouseDragPosition.x - _mouseDownPosition.x) * mouseSensitivity;

            _aroundCameraController.Rotate(new Vector3(-xRotation, yRotation));

            _mouseDownPosition = Vector3.Lerp(_mouseDownPosition, _mouseDragPosition, 0.05f);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _pointerDown = true;
            _mouseDownPosition = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _pointerDown = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
//            Ray ray = _aroundCameraController.Camera.ScreenPointToRay(Input.mousePosition);
//            if (!_blocksController.OnBlockHit(ray))
//            {
                _mouseDragPosition = Input.mousePosition;
//            }
//            else
//            {
//                _mouseDownPosition = Input.mousePosition;
//                _mouseDragPosition = _mouseDownPosition;
//            }
        }

        public void StopDrag()
        {
            _mouseDownPosition = Input.mousePosition;
            _mouseDragPosition = _mouseDownPosition;
        }
    }
}