using UnityEngine;
using UnityEngine.UI;

namespace MineMiner
{
    public class MapEditorUI : MonoBehaviour
    {
        [SerializeField] private Button _newLevelButton;

        public Button NewLevelButton => _newLevelButton;
    }
}