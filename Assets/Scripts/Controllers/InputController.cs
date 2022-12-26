using UnityEngine;
using UnityEngine.EventSystems;

namespace MineMiner
{
    public class InputController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Vector3 _mouseDownPosition;

        private Vector2 _mouseDragPosition = Vector2.zero;

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

        public void Tick()
        {
            _mouseDownPosition = Vector3.Lerp(_mouseDownPosition, _mouseDragPosition, 0.05f);
        }

        public Vector3 MouseDownPosition
        {
            get { return _mouseDownPosition; }
        }

        public Vector2 MouseDragPosition
        {
            get { return _mouseDragPosition; }
        }
    }
}