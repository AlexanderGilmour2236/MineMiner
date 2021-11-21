using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MineSceneNavigator : Navigator
    {
        [Inject] private ICameraController _cameraController;
        [Inject] private BlocksController _blocksController;
        [Inject] private LevelCameraController _levelCameraController;

        [Inject] private MineSceneAccessor _mineSceneAccessor;
        
        public override void Go()
        {
            base.Go();
            InitControllers();
        }

        public override void Tick()
        {
            _levelCameraController.Tick();
        }

        private void InitControllers()
        {
            _blocksController.Init(_mineSceneAccessor.CracksBlockPrefab, _mineSceneAccessor.LevelCenterTransform, _mineSceneAccessor.LevelParent);
            _blocksController.onBlockDestroy += OnOnBlockDestroy;
            _blocksController.onBlockHit += OnBlockHit;
            _blocksController.SetAllBlocks(_mineSceneAccessor.LevelParent);
            
            _cameraController.SetTargetPoint(_blocksController.LevelCenterTransform);
        }

        private void OnBlockHit(BlockView blockView)
        {
            _levelCameraController.StopDrag();
        }

        private void OnOnBlockDestroy(BlockView block)
        {
        }


    }
}