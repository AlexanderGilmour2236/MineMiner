using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Transform _movingTransform;
    [SerializeField] private Transform _showPositionTransform;
    [SerializeField] private Transform _hidePositionTransform;
    [SerializeField] private float _openCloseDuration = 0.4f;
    [SerializeField] private Ease _ease;

    private Tween _openCloseTween;

    void Start()
    {
        _openButton.onClick.AddListener(OnOpenButtonClick);
        _closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnOpenButtonClick()
    {
        DestroyTween();
        _openCloseTween = _movingTransform.DOMove(_showPositionTransform.position, _openCloseDuration).SetEase(_ease);
    }

    private void DestroyTween()
    {
        if (_openCloseTween != null)
        {
            _openCloseTween.Kill();
        }

        _openCloseTween = null;
    }

    private void OnCloseButtonClick()
    {
        DestroyTween();
        _openCloseTween = _movingTransform.DOMove(_hidePositionTransform.position, _openCloseDuration).SetEase(_ease);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
