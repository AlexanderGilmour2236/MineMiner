using UnityEngine;

namespace MineMiner
{
    [CreateAssetMenu(fileName = "Level##Config", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private TextAsset _levelFile;

        public TextAsset LevelFile
        {
            get { return _levelFile; }
        }
    }
}