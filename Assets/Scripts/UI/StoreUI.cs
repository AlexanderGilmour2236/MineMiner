using HidableElement;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private PositionHidableElement _storePositionHidableElement;

    private ShowHideTweenController _positionShowHideTweenController =
        new ShowHideTweenController(new PositionShowHideTweenStrategy());
    
    void Start()
    {
        _openButton.onClick.AddListener(OnOpenButtonClick);
        _closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnOpenButtonClick()
    {
        _positionShowHideTweenController.show(_storePositionHidableElement);
    }
    
    private void OnCloseButtonClick()
    {
        _positionShowHideTweenController.hide(_storePositionHidableElement);
    }
}
