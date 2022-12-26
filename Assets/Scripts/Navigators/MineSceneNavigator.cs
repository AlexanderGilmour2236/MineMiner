using System;
using ResourcesProvider;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MineMiner
{
    public class MineSceneNavigator : SceneNavigator
    {
        private BlocksController _blocksController;
        private LevelCameraController _levelCameraController;

        private MineSceneAccessor _mineSceneAccessor;
        private BlocksFactory _blocksFactory;
        private LevelsFilesConfig _levelsFilesConfig;
        private LevelGenerator _levelGenerator;

        private CameraRaycastController _cameraRaycastController;
        private bool _isNavigatorInitiated;
        private readonly Player _player;
        private MineSceneUI _mineSceneUI;

        private MineSceneState _mineSceneState;
        
        public MineSceneNavigator(String sceneName, Player player) : base(sceneName)
        {
            _player = player;
            SubscribePlayer(_player);
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            base.OnSceneLoaded(scene, loadSceneMode);
            _mineSceneAccessor = Object.FindObjectOfType<MineSceneAccessor>();

            InitControllers();
            InitUI();
            SetMainSceneState(MineSceneState.MainMenu);
        }

        private void SetMainSceneState(MineSceneState mainMenuState)
        {
            _mineSceneState = mainMenuState;
            switch (mainMenuState)
            {
                case MineSceneState.MainMenu:
                    _cameraRaycastController.SetIsEnabled(false);
                    _mineSceneUI.ShowMainMenuUI();
                    _levelCameraController.SetGetLevelRotationStrategy(new TimeGetLevelRotationStrategy(30));
                    break;
                case MineSceneState.EndlessMode:
                    _cameraRaycastController.SetIsEnabled(true);
                    _mineSceneUI.HideMainMenuUI();
                    _levelCameraController.SetGetLevelRotationStrategy(new DragGetLevelRotationStrategy());
                    break;
                case MineSceneState.BlockFromStore:
                    _cameraRaycastController.SetIsEnabled(true);
                    _mineSceneUI.HideMainMenuUI();
                    _levelCameraController.SetGetLevelRotationStrategy(new DragGetLevelRotationStrategy());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mainMenuState), mainMenuState, null);
            }
        }

        private void SubscribePlayer(Player player)
        {
            player.PlayerResources.onResourceChange += OnPlayerResourceChanged;
        }

        private void OnPlayerResourceChanged(PlayerResource playerResource)
        {
            _mineSceneUI.UpdatePlayerResource(playerResource);
            _player.SavePlayerData();
        }

        private void InitUI()
        {
            _mineSceneUI = _mineSceneAccessor.MineSceneUI;
            _mineSceneUI.Init(_blocksFactory);
            SubscribeMainSceneUI(_mineSceneUI);
        }

        private void InitControllers()
        {
            _blocksFactory = _mineSceneAccessor.BlocksFactory;
            _blocksController = new BlocksController(_blocksFactory, _player);
            _levelGenerator = new LevelGenerator(_blocksController, _blocksFactory);
            
            _levelCameraController = _mineSceneAccessor.CameraLevelController;
            _cameraRaycastController = new CameraRaycastController(_levelCameraController.Camera, 1 << LayerMask.NameToLayer(TagManager.DestroyableBlocksLayer));
            _cameraRaycastController.onHit += HitBlockByRaycast;
            _blocksController.Init(_mineSceneAccessor.CracksBlockPrefab, _mineSceneAccessor.LevelCenterTransform, _mineSceneAccessor.LevelParent);
            _blocksController.onBlockDestroy += OnBlockDestroy;
            CreateLevelBlocks();
            _levelCameraController.Init();
            _levelCameraController.SetGetLevelRotationStrategy(new TimeGetLevelRotationStrategy(30));
            
            _isNavigatorInitiated = true;
        }

        private void SubscribeMainSceneUI(MineSceneUI mineSceneUI)
        {
            mineSceneUI.onGoToMainMenuClick += GoToMainMenu;
            mineSceneUI.onGoToEndlessModeClick += GoToEndlessMode;
        }

        private void UnsubscribeMainSceneUI(MineSceneUI mineSceneUI)
        {
            mineSceneUI.onGoToMainMenuClick += GoToMainMenu;
            mineSceneUI.onGoToEndlessModeClick += GoToEndlessMode;
        }

        private void GoToMainMenu()
        {
            SetMainSceneState(MineSceneState.MainMenu);
        }

        private void GoToEndlessMode()
        {
            SetMainSceneState(MineSceneState.EndlessMode);
        }


        public override void Tick()
        {
            if (!_isNavigatorInitiated)
            {
                return;
            }
            
            _cameraRaycastController.Tick();
            _blocksController.Tick();
            _levelCameraController.Tick();
        }

        private void HitBlockByRaycast(RaycastHit raycastHit)
        {
            DestroyableBlockView destroyableBlockView = raycastHit.collider.GetComponent<DestroyableBlockView>();
            _blocksController.OnHit(destroyableBlockView);
        }

        private void CreateLevelBlocks()
        {
            LoadLevelFromGenerator();

            _blocksController.SetCenterPoint();
            _levelCameraController.setPivotPosition(_blocksController.LevelCenterTransform.position, true);
        }

        private void LoadLevelFromGenerator()
        {
            _levelGenerator.GenerateLevel(Random.Range(3, 6),Random.Range(5,10), Random.Range(3, 7));
        }

        private void LoadLevelFromJson(int levelIndex)
        {
            LevelData levelData = SaveLoadController.LoadObjectFromString(
                _levelsFilesConfig.LevelConfigs[levelIndex].LevelFile.text,
                (json) => new LevelData(json));

            foreach (BlockData blockData in levelData.BlockDatas)
            {
                blockData.BlockMetaData = _blocksFactory.GetBlockMetaData(blockData.BlockId); 
                if (blockData.BlockDataType == BlockDataType.Destroyable)
                {
                    DestroyableBlockData destroyableBlockData = (DestroyableBlockData) blockData;
                    _blocksController.CreateBlock(destroyableBlockData.Position, (DestroyableBlockMetaData)blockData.BlockMetaData);
                }
            }
        }

        private void OnBlockDestroy(BlockView block, int droppedBlocksCount)
        {
            _blocksController.SetCenterPoint();
            _player.PlayerResources.AddResource(block.MetaData.Id, droppedBlocksCount);
            _levelCameraController.setPivotPosition(_blocksController.LevelCenterTransform.position);
            
            if (_mineSceneState == MineSceneState.EndlessMode && _blocksController.CurrentBlocks.Count == 0)
            {
                CreateLevelBlocks();
            }
        }


    }
}