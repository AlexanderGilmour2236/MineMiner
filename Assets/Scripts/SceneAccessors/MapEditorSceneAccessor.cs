using UnityEngine;

namespace MineMiner
{
    public class MapEditorSceneAccessor : MonoBehaviour
    {
        [SerializeField] private MapEditorUI _mapEditorUI;
        [SerializeField] private Transform _levelStartPoint;

        public MapEditorUI MapEditorUI => _mapEditorUI;
        public Transform LevelStartPoint => _levelStartPoint;
    }
}