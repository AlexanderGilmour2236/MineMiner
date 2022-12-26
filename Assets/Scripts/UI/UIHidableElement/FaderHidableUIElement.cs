using UnityEngine;

namespace HidableElement
{
    public class FaderHidableUIElement : HidableUIElement
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public CanvasGroup canvasGroup
        {
            get { return _canvasGroup; }
        }
    }
}