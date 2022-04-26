using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MineSceneNavigator : Navigator
    {
        [Inject] private BlocksController _blocksController;
        [Inject] private LevelCameraController _levelCameraController;

        [Inject] private MineSceneAccessor _mineSceneAccessor;

        private bool _isNaviatorInitiated;

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
            
            _blocksController.Tick();
            _levelCameraController.Tick();
        }

        private void InitControllers()
        {
            _blocksController.Init(_mineSceneAccessor.CracksBlockPrefab, _mineSceneAccessor.LevelCenterTransform, _mineSceneAccessor.LevelParent);
            _blocksController.onBlockDestroy += OnOnBlockDestroy;
            _blocksController.onBlockHit += OnBlockHit;
            _blocksController.SetAllBlocksFromLevelGameObjects(_mineSceneAccessor.LevelParent);

            _levelCameraController.Init();
            
            _isNaviatorInitiated = true;
        }

        private void OnBlockHit(DestroyableBlockView blockView)
        {
            if (blockView.Hit(Time.deltaTime * ((GameApp)App.Instance()).Player.Damage))
            {
                return;
            }
            _levelCameraController.StopDrag();
        }

        private void OnOnBlockDestroy(BlockView block)
        {
        }


    }
}