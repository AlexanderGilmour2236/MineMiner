using System;
using DG.Tweening;

namespace HidableElement
{
    public class FaderShowHideTweenStrategy : ShowHideTweenStrategy
    {
        public override Tween show(HidableUIElement hidableUIElement, bool resetValue = false, Action onComplete = null)
        {
            FaderHidableUIElement faderHidableUIElement = (FaderHidableUIElement)hidableUIElement;
            
            if (resetValue)
            {
                faderHidableUIElement.canvasGroup.alpha = 0;
            }
            return faderHidableUIElement.canvasGroup.DOFade(1, faderHidableUIElement.duration).OnComplete(() => onComplete?.Invoke());
        }

        public override Tween hide(HidableUIElement hidableUIElement, bool resetValue = false, Action onComplete = null)
        {
            FaderHidableUIElement faderHidableUIElement = (FaderHidableUIElement)hidableUIElement;

            if (resetValue)
            {
                faderHidableUIElement.canvasGroup.alpha = 1;
            }
            return faderHidableUIElement.canvasGroup.DOFade(0, faderHidableUIElement.duration).OnComplete(() => onComplete?.Invoke());
        }
    }
}