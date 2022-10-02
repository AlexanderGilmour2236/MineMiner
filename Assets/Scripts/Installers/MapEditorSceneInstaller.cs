using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MapEditorSceneInstaller : MonoInstaller
    {
        [SerializeField] private MapEditorSceneAccessor _mapEditorSceneAccessor;
        [SerializeField] private MapEditorCameraController _mapEditorCameraController;
        [SerializeField] private BlocksFactory _blocksFactory;

        public override void InstallBindings()
        {
            Container.Bind(typeof(App), typeof(ITickable)).To<MapEditorApp>().AsSingle().NonLazy();
            
            Container.Bind<BlocksFactory>().FromInstance(_blocksFactory);
            Container.Bind<BlocksController>().AsSingle().NonLazy();
            Container.Bind<MapEditorSceneAccessor>().FromInstance(_mapEditorSceneAccessor);
            Container.Bind<MapEditorCameraController>().FromInstance(_mapEditorCameraController);
            Container.Bind<MapEditorNavigator>().AsSingle().NonLazy();
            Container.Bind<LevelGenerator>().AsSingle().NonLazy();
        }
    }
}