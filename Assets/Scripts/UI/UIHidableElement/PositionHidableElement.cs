using UnityEngine;

namespace HidableElement
{
    public class PositionHidableElement : HidableUIElement
    {
        [SerializeField] private Transform _movingTransform;
        [SerializeField] private Transform _hiddenPositionTransform;
        [SerializeField] private Transform _showedPositionTransform;

        public Transform movingTransform
        {
            get { return _movingTransform; }
        }

        public Transform hiddenPositionTransform
        {
            get { return _hiddenPositionTransform; }
        }

        public Transform showedPositionTransform
        {
            get { return _showedPositionTransform; }
        }
    }
}