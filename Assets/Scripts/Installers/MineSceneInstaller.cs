using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MineSceneInstaller : MonoInstaller
    {
        [SerializeField] private MineSceneAccessor _mineSceneAccessor;
        [SerializeField] private AroundCameraMovementStrategy aroundCameraMovementStrategy;
        [SerializeField] private LevelCameraController _levelCameraController;
        [SerializeField] private BlocksFactory _blocksFactory;

        public override void InstallBindings()
        {
            Container.Bind<App>().To<GameApp>().AsSingle().NonLazy();

            Container.Bind<MineSceneNavigator>().AsSingle().NonLazy();
            Container.Bind<LevelCameraController>().FromInstance(_levelCameraController);
            Container.Bind<BlocksController>().AsSingle().NonLazy();
            Container.Bind<ICameraMovementStrategy>().FromInstance(aroundCameraMovementStrategy);
            Container.Bind<BlocksFactory>().FromInstance(_blocksFactory);
            
            Container.Bind<MineSceneAccessor>().FromInstance(_mineSceneAccessor);
        }
    }
}