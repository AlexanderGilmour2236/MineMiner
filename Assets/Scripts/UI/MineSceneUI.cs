using System;
using System.Collections.Generic;
using HidableElement;
using ResourcesProvider;
using UnityEngine;

namespace MineMiner
{
    public class MineSceneUI : MonoBehaviour
    {
        [SerializeField] private ResourceIndicator _goldsResourceIndicator;
        [SerializeField] private GameHUD _gameHUD;
        
        [SerializeField] private PositionHidableElement _shopUIHidableElement;
        [SerializeField] private FaderHidableUIElement _gameNameTextHidableElement;
        [SerializeField] private FaderHidableUIElement _tapToEndlessBlocksTextHidableElement;
        [SerializeField] private PositionHidableElement _inventoryButtonHidableElement;
        [SerializeField] private PositionHidableElement _pauseButtonHidableElement;

        public event Action onGoToMainMenuClick;
        public event Action onGoToEndlessModeClick;

        private ShowHideTweenController _positionShowHideTweenController =
            new ShowHideTweenController(new PositionShowHideTweenStrategy());
        private ShowHideTweenController _faderShowHideTweenController =
            new ShowHideTweenController(new FaderShowHideTweenStrategy());
        
        private Dictionary<BlockId, ResourceIndicator> _blockIdToResourceIndicator =
            new Dictionary<BlockId, ResourceIndicator>();

        public void Init(BlocksFactory blocksFactory)
        {
            _blockIdToResourceIndicator[BlockId.Golds] = _goldsResourceIndicator;
            _goldsResourceIndicator.SetSprite(blocksFactory.GetBlockMetaData(BlockId.Golds).Sprite);
            _goldsResourceIndicator.SetValueText(PlayerPrefs.GetInt(PlayerPrefsKeys.PlayersGolds).ToString());
            SubscribeHUD(_gameHUD);
        }

        private void SubscribeHUD(GameHUD gameHUD)
        {
            gameHUD.onClick += StartEndlessMode;
            gameHUD.onPauseClick += ReturnToMainMenu;
        }
        
        private void UnsubscribeHUD(GameHUD gameHUD)
        {
            gameHUD.onClick -= StartEndlessMode;
            gameHUD.onPauseClick -= ReturnToMainMenu;
        }

        private void StartEndlessMode()
        {
            HideMainMenuUI();
            onGoToEndlessModeClick?.Invoke();
        }

        private void ReturnToMainMenu()
        {
            ShowMainMenuUI();
            onGoToMainMenuClick?.Invoke();
        }

        public void UpdatePlayerResource(PlayerResource playerResource)
        {
            if (_blockIdToResourceIndicator.ContainsKey(playerResource.BlockId))
            {
                _blockIdToResourceIndicator[playerResource.BlockId].SetValueText(playerResource.Amount.ToString());
            }
        }

        public void ShowMainMenuUI()
        {
            _gameHUD.SetIsBlockingRaycast(true);
            _positionShowHideTweenController.show(_shopUIHidableElement);
            _positionShowHideTweenController.show(_inventoryButtonHidableElement);
            _positionShowHideTweenController.hide(_pauseButtonHidableElement);
            _faderShowHideTweenController.show(_gameNameTextHidableElement);
            _faderShowHideTweenController.show(_tapToEndlessBlocksTextHidableElement);
        }
        
        public void HideMainMenuUI()
        {
            _gameHUD.SetIsBlockingRaycast(false);
            _positionShowHideTweenController.hide(_shopUIHidableElement);
            _positionShowHideTweenController.hide(_inventoryButtonHidableElement);
            _positionShowHideTweenController.show(_pauseButtonHidableElement);
            _faderShowHideTweenController.hide(_gameNameTextHidableElement);
            _faderShowHideTweenController.hide(_tapToEndlessBlocksTextHidableElement);
        }
    }
}