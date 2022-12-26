using System;
using System.Collections.Generic;
using DG.Tweening;

using UnityEngine;

namespace HidableElement
{
    public class ShowHideTweenController
    {
        private ShowHideTweenStrategy _showHideTweenStrategy;
        private Tween _fadeTweeen;
        
        private Dictionary<HidableUIElement, Tween> _hidableUIElementToTween = new Dictionary<HidableUIElement, Tween>();

        public ShowHideTweenController(ShowHideTweenStrategy showHideTweenStrategy)
        {
            _showHideTweenStrategy = showHideTweenStrategy;
        }
        
        public void show(HidableUIElement hidableUIElement, bool resetValue = false, Action onComplete = null)
        {
            killTweenIfExists(hidableUIElement);
            _hidableUIElementToTween[hidableUIElement] = _showHideTweenStrategy.show(hidableUIElement, resetValue, onComplete);
            hidableUIElement.isHidden = false;
        }

        public void hide(HidableUIElement hidableUIElement, bool resetValue = false, Action onComplete = null)
        {
            killTweenIfExists(hidableUIElement);
            _hidableUIElementToTween[hidableUIElement] = _showHideTweenStrategy.hide(hidableUIElement, resetValue, onComplete);;
            hidableUIElement.isHidden = true;
        }

        public void killTweenIfExists(HidableUIElement hidableUIElement)
        {
            if (_hidableUIElementToTween.ContainsKey(hidableUIElement))
            {
                Tween fadeTween = _hidableUIElementToTween[hidableUIElement];
                fadeTween.Kill();
                _hidableUIElementToTween.Remove(hidableUIElement);
            }
        }
    }
}