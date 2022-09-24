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
            Container.BindInterfacesTo<MineMinerApp>().AsSingle().NonLazy();

            Container.Bind<LevelCameraController>().FromInstance(_levelCameraController);
            Container.Bind<MineSceneNavigator>().FromNew().AsSingle().WithArguments(_levelCameraController).NonLazy();
            Container.Bind<BlocksController>().AsSingle().NonLazy();
            Container.Bind<ICameraMovementStrategy>().FromInstance(aroundCameraMovementStrategy);
            Container.Bind<BlocksFactory>().FromInstance(_blocksFactory);
            
            Container.Bind<MineSceneAccessor>().FromInstance(_mineSceneAccessor);
        }
    }
}