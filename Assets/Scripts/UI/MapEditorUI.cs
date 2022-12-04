using UnityEngine;
using UnityEngine.UI;

namespace MineMiner
{
    public class MapEditorUI : MonoBehaviour
    {
        [SerializeField] private Button _newLevelButton;
        [SerializeField] private Button _saveLevelButton;
        [SerializeField] private Button _loadLevelButton;
        [SerializeField] private Button _generateLevelButton;

        public Button SaveLevelButton
        {
            get { return _saveLevelButton; }
        }

        public Button NewLevelButton
        {
            get { return _newLevelButton; }
        }

        public Button LoadLevelButton
        {
            get { return _loadLevelButton; }
        }

        public Button GenerateLevelButton
        {
            get { return _generateLevelButton; }
        }
    }
}