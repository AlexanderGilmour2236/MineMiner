using System;
using DG.Tweening;
using UnityEngine;

namespace HidableElement
{
    public abstract class HidableUIElement : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _timeIndependent = false;

        private bool _isHidden = true;

        public float duration
        {
            get { return _duration; }
        }

        public Ease ease
        {
            get { return _ease; }
        }

        public bool isHidden
        {
            get { return _isHidden; }
            set { _isHidden = value; }
        }

        public bool timeIndependent
        {
            get { return _timeIndependent; }
        }
    }
}