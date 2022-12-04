using UnityEngine;

namespace MineMiner
{
    public class MapEditorSceneInstaller 
    {
        [SerializeField] private MapEditorSceneAccessor _mapEditorSceneAccessor;
        [SerializeField] private MapEditorCameraController _mapEditorCameraController;
        [SerializeField] private BlocksFactory _blocksFactory;
    }
}