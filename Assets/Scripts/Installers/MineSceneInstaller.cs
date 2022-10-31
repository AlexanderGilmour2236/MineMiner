using ResourcesProvider;
using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MineSceneInstaller : MonoInstaller
    {
        [SerializeField] private MineSceneAccessor _mineSceneAccessor;
        [SerializeField] private LevelCameraController _levelCameraController;
        [SerializeField] private BlocksFactory _blocksFactory;
        [SerializeField] private LevelsFilesConfig _levelsFilesConfig;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MineMinerApp>().AsSingle().NonLazy();

            Container.Bind<LevelCameraController>().FromInstance(_levelCameraController);
            Container.Bind<MineSceneNavigator>().FromNew().AsSingle().WithArguments(_levelCameraController).NonLazy();
            Container.Bind<BlocksController>().AsSingle().NonLazy();
            Container.Bind<BlocksFactory>().FromInstance(_blocksFactory);
            Container.Bind<LevelsFilesConfig>().FromInstance(_levelsFilesConfig);
            Container.Bind<LevelGenerator>().AsSingle().NonLazy();
            Container.Bind<MineSceneAccessor>().FromInstance(_mineSceneAccessor);
        }
    }
}