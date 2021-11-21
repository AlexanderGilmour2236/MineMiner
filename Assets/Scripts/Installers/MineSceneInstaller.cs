using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MineSceneInstaller : MonoInstaller
    {
        [SerializeField] private MineSceneAccessor mineSceneAccessor;
        [SerializeField] private AroundCameraController aroundCameraController;
        [SerializeField] private LevelCameraController levelCameraController;
        [SerializeField] private BlocksFactory blocksFactory;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<App>().AsSingle().NonLazy();

            Container.Bind<MineSceneNavigator>().AsSingle().NonLazy();
            Container.Bind<LevelCameraController>().FromInstance(levelCameraController);
            Container.Bind<BlocksController>().AsSingle().NonLazy();
            Container.Bind<ICameraController>().FromInstance(aroundCameraController);
            Container.Bind<BlocksFactory>().FromInstance(blocksFactory);
            
            Container.Bind<MineSceneAccessor>().FromInstance(mineSceneAccessor);
        }
    }
}