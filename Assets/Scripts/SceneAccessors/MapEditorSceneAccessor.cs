using UnityEngine;

namespace MineMiner
{
    public class MapEditorSceneAccessor : MonoBehaviour
    {
        [SerializeField] private MapEditorUI _mapEditorUI;
        [SerializeField] private Transform _levelStartPoint;
        [SerializeField] private DestroyableBlockView _currentSampleBlockView;

        public MapEditorUI MapEditorUI => _mapEditorUI;
        public Transform LevelStartPoint => _levelStartPoint;

        public DestroyableBlockView CurrentSampleBlockView => _currentSampleBlockView; 
    }
}