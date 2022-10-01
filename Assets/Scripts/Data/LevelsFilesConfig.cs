using UnityEngine;

namespace MineMiner
{
    [CreateAssetMenu(fileName = "LevelsFilesConfig", menuName = "Configs/LevelsFilesConfig")]
    public class LevelsFilesConfig : ScriptableObject
    {
        [SerializeField] private LevelConfig[] levelConfigs;

        public LevelConfig[] LevelConfigs
        {
            get { return levelConfigs; }
        }
    }
}