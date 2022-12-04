using UnityEngine;
using UnityEngine.UI;

namespace MineMiner
{
    public class MapEditorSceneAccessor : MonoBehaviour
    {
        [SerializeField] private MapEditorUI _mapEditorUI;
        [SerializeField] private Transform _levelStartPoint;
        [SerializeField] private DestroyableBlockView _currentSampleBlockView;
        [SerializeField] public Image _noizeImage;
        [SerializeField] private BlocksFactory _blocksFactory;
        [SerializeField] private MapEditorCameraController _mapEditorCameraController;

        public MapEditorUI MapEditorUI
        {
            get { return _mapEditorUI; }
        }

        public Transform LevelStartPoint
        {
            get { return _levelStartPoint; }
        }

        public DestroyableBlockView CurrentSampleBlockView
        {
            get { return _currentSampleBlockView; }
        }

        public BlocksFactory BlocksFactory
        {
            get { return _blocksFactory; }
        }

        public MapEditorCameraController MapEditorCameraController
        {
            get { return _mapEditorCameraController; }
        }
    }
}