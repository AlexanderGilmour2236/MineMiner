using UnityEngine;

namespace MineMiner
{
    public class MineSceneInstaller
    {
        [SerializeField] private MineSceneAccessor _mineSceneAccessor;
        [SerializeField] private LevelCameraController _levelCameraController;
        [SerializeField] private BlocksFactory _blocksFactory;
        [SerializeField] private LevelsFilesConfig _levelsFilesConfig;
    }
}