using System;
using DG.Tweening;

namespace HidableElement
{
    public abstract class ShowHideTweenStrategy
    {
        public virtual Tween show(HidableUIElement hidableUIElement, bool resetValue = false, Action onComplete = null)
        {
            return null;
        }

        public virtual Tween hide(HidableUIElement hidableUIElement, bool resetValue = false, Action onComplete = null)
        {
            return null;
        }
    }
}