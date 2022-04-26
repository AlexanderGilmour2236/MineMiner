using UnityEngine;
using UnityEngine.UI;

namespace MineMiner
{
    public class MapEditorUI : MonoBehaviour
    {
        [SerializeField] private Button _newLevelButton;
        [SerializeField] private Button _saveLevelButton;
        [SerializeField] private Button _loadLevelButton;
        
        public Button SaveLevelButton => _saveLevelButton;
        public Button NewLevelButton => _newLevelButton;
        public Button LoadLevelButton => _loadLevelButton;
    }
}