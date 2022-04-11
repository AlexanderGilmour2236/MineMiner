using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MineSceneInstaller : MonoInstaller
    {
        [SerializeField] private MineSceneAccessor _mineSceneAccessor;
        [SerializeField] private AroundCameraController _aroundCameraController;
        [SerializeField] private LevelCameraController _levelCameraController;
        [SerializeField] private BlocksFactory _blocksFactory;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<App>().AsSingle().NonLazy();

            Container.Bind<MineSceneNavigator>().AsSingle().NonLazy();
            Container.Bind<LevelCameraController>().FromInstance(_levelCameraController);
            Container.Bind<BlocksController>().AsSingle().NonLazy();
            Container.Bind<ICameraController>().FromInstance(_aroundCameraController);
            Container.Bind<BlocksFactory>().FromInstance(_blocksFactory);
            
            Container.Bind<MineSceneAccessor>().FromInstance(_mineSceneAccessor);
        }
    }
}