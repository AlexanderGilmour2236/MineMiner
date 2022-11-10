using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MineMiner
{
    public class ResourceIndicator : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _valueText;

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetValueText(String text)
        {
            _valueText.text = text;
        }
    }
}