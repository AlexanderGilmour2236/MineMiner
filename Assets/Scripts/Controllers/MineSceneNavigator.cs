using System;
using System.Collections.Generic;
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
        
        private CameraRaycastController _cameraRaycastController;
        private bool _isNaviatorInitiated;

        [Inject]
        public MineSceneNavigator(LevelCameraController levelCameraController)
        {
            _cameraRaycastController = new CameraRaycastController(levelCameraController.Camera, 1 << LayerMask.NameToLayer(TagManager.DestroyableBlocksLayer));
        }
        
        public override void Go()
        {
            base.Go();
            InitControllers();
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
            LevelData levelData = SaveLoadController.LoadObjectFromString(
                _levelsFilesConfig.LevelConfigs[Random.Range(0, _levelsFilesConfig.LevelConfigs.Length)].LevelFile.text,
                (json) => new LevelData(json));
            
            int blockIndex = 0;
            foreach (BlockData blockData in levelData.BlockDatas)
            {
                blockData.BlockMetaData = _blocksFactory.GetBlockMetaData(blockData.BlockId); 
                if (blockData.BlockDataType == BlockDataType.Destroyable)
                {
                    DestroyableBlockData destroyableBlockData = (DestroyableBlockData) blockData;
                    _blocksController.CreateBlock(destroyableBlockData.Position, (DestroyableBlockMetaData)blockData.BlockMetaData);
                }

                blockIndex++;
            }

            _blocksController.SetCenterPoint();
            _levelCameraController.setPivotPosition(_blocksController.LevelCenterTransform.position);
        }

        private void OnBlockHit(DestroyableBlockView blockView)
        {
            if (blockView.Hit(Time.deltaTime * ((MineMinerApp)App.Instance()).Player.Damage))
            {
                return;
            }
            _levelCameraController.StopDrag();
        }

        private void OnOnBlockDestroy(BlockView block)
        {
            _blocksController.SetCenterPoint();
            _levelCameraController.setPivotPosition(_blocksController.LevelCenterTransform.position);
        }


    }
}