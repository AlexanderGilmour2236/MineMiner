using System;
using DG.Tweening;
using UnityEngine;

namespace HidableElement
{
    public class PositionShowHideTweenStrategy : ShowHideTweenStrategy
    {
        public override Tween show(HidableUIElement hidableUIElement, bool resetValue = false, Action onComplete = null)
        {
            PositionHidableElement positionHidableElement = (PositionHidableElement) hidableUIElement;
            
            Transform movingTransform = positionHidableElement.movingTransform;

            if (resetValue)
            {
                movingTransform.position = positionHidableElement.hiddenPositionTransform.position;
            }

            return movingTransform.DOMove(positionHidableElement.showedPositionTransform.position,
                positionHidableElement.duration)
                .SetEase(positionHidableElement.ease)
                .OnComplete(() => onComplete?.Invoke())
                .SetUpdate(hidableUIElement.timeIndependent);
        }

        public override Tween hide(HidableUIElement hidableUIElement, bool resetValue = false, Action onComplete = null)
        {
            PositionHidableElement positionHidableElement = (PositionHidableElement) hidableUIElement;
            
            Transform movingTransform = positionHidableElement.movingTransform;

            if (resetValue)
            {
                movingTransform.position = positionHidableElement.showedPositionTransform.position;
            }

            return movingTransform.DOMove(positionHidableElement.hiddenPositionTransform.position,
                positionHidableElement.duration)
                .SetEase(positionHidableElement.ease)
                .OnComplete(() => onComplete?.Invoke())
                .SetUpdate(hidableUIElement.timeIndependent);
        }
    }
}