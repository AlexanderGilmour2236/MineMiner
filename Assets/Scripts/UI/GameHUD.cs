using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MineMiner
{
    public class GameHUD : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _raycastBlocker;
        [SerializeField] private Button _pauseButton;
        
        public event Action onClick;
        public event Action onPauseClick;

        private void Awake()
        {
            _pauseButton.onClick.AddListener(PauseButtonClick);
        }

        private void PauseButtonClick()
        {
            onPauseClick?.Invoke();
        }

        public void SetIsBlockingRaycast(bool isBlockingRaycast)
        {
            _raycastBlocker.raycastTarget = isBlockingRaycast;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}