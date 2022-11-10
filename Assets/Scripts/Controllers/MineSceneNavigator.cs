using System;
using System.Collections.Generic;
using ResourcesProvider;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace MineMiner
{
    public class MineSceneNavigator : Navigator
    {
        [Inject] private BlocksController _blocksController;
        [Inject] private LevelCameraController _levelCameraController;

        [Inject] private MineSceneAccessor _mineSceneAccessor;
        [Inject] private BlocksFactory _blocksFactory;
        [Inject] private LevelsFilesConfig _levelsFilesConfig;
        [Inject] private LevelGenerator _levelGenerator;

        private CameraRaycastController _cameraRaycastController;
        private bool _isNaviatorInitiated;
        private readonly Player _player;
        private MineSceneUI _mineSceneUI;


        [Inject]
        public MineSceneNavigator(LevelCameraController levelCameraController, Player player)
        {
            _player = player;
            SubscribePlayer(_player);
            _cameraRaycastController = new CameraRaycastController(levelCameraController.Camera, 1 << LayerMask.NameToLayer(TagManager.DestroyableBlocksLayer));
        }

        private void SubscribePlayer(Player player)
        {
            player.PlayerResources.onResourceChange += OnPlayerResourceChanged;
        }

        private void OnPlayerResourceChanged(PlayerResource playerResource)
        {
            _mineSceneUI.UpdatePlayerResource(playerResource);
        }

        public override void Go()
        {
            base.Go();
            InitControllers();
            InitUI();
        }

        private void InitUI()
        {
            _mineSceneUI = _mineSceneAccessor.MineSceneUI;
            _mineSceneUI.Init(_blocksFactory);
        }
        

        public override void Tick()
        {
            if (!_isNaviatorInitiated)
            {
                return;
            }
            
            _cameraRaycastController.Tick();
            _blocksController.Tick();
            _levelCameraController.Tick();
        }

        private void InitControllers()
        {
            _cameraRaycastController.onHit += HitBlockByRaycast;
            _blocksController.Init(_mineSceneAccessor.CracksBlockPrefab, _mineSceneAccessor.LevelCenterTransform, _mineSceneAccessor.LevelParent);
            _blocksController.onBlockDestroy += OnOnBlockDestroy;
            _blocksController.onBlockHit += OnBlockHit;
            CreateLevelBlocks();
            _levelCameraController.Init();
            
            _isNaviatorInitiated = true;
        }

        private void HitBlockByRaycast(RaycastHit raycastHit)
        {
            DestroyableBlockView destroyableBlockView = raycastHit.collider.GetComponent<DestroyableBlockView>();
            _blocksController.OnHit(destroyableBlockView);
        }

        private void CreateLevelBlocks()
        {
            // LevelData levelData = SaveLoadController.LoadObjectFromString(
            //     _levelsFilesConfig.LevelConfigs[Random.Range(0, _levelsFilesConfig.LevelConfigs.Length)].LevelFile.text,
            //     (json) => new LevelData(json));
            //
            //
            // int blockIndex = 0;
            // foreach (BlockData blockData in levelData.BlockDatas)
            // {
            //     blockData.BlockMetaData = _blocksFactory.GetBlockMetaData(blockData.BlockId); 
            //     if (blockData.BlockDataType == BlockDataType.Destroyable)
            //     {
            //         DestroyableBlockData destroyableBlockData = (DestroyableBlockData) blockData;
            //         _blocksController.CreateBlock(destroyableBlockData.Position, (DestroyableBlockMetaData)blockData.BlockMetaData);
            //     }
            //
            //     blockIndex++;
            // }
            
            _levelGenerator.GenerateLevel(Random.Range(3, 6),Random.Range(5,10), Random.Range(3, 7));

            _blocksController.SetCenterPoint();
            _levelCameraController.setPivotPosition(_blocksController.LevelCenterTransform.position);
        }

        private void OnBlockHit(DestroyableBlockView blockView)
        {
            if (blockView.Hit(Time.deltaTime * _player.Damage))
            {
                DestroyableBlockMetaData destroyableBlockMetaData = blockView.DestroyableBlockMetaData;
                return;
            }

            _levelCameraController.StopDrag();
            
        }

        private void OnOnBlockDestroy(BlockView block, int droppedBlocksCount)
        {
            _blocksController.SetCenterPoint();
            _player.PlayerResources.AddResource(block.MetaData.Id, droppedBlocksCount);
            _levelCameraController.setPivotPosition(_blocksController.LevelCenterTransform.position);
        }


    }
}