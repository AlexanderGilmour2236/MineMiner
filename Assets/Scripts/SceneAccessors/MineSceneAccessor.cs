using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MineMiner
{
    public class MineSceneAccessor : MonoBehaviour
    {
        [SerializeField] private Transform _levelParent;
        [SerializeField] private Transform _levelCenterTransform;
        [SerializeField] private CracksBlock _cracksBlockPrefab;
        [SerializeField] private Button _rButton;
        [SerializeField] private MineSceneUI _mineSceneUI;
        [SerializeField] private LevelCameraController _cameraLevelController;
        [SerializeField] private BlocksFactory _blocksFactory;


        public void Awake()
        {
            // _rButton.onClick.AddListener(() => App.RestartApp());
        }

        public Transform LevelParent
        {
            get { return _levelParent; }
        }

        public Transform LevelCenterTransform 
        {
            get { return _levelCenterTransform; } 
        }

        public CracksBlock CracksBlockPrefab
        {
            get { return _cracksBlockPrefab; }
        }
        
        public MineSceneUI MineSceneUI
        {
            get { return _mineSceneUI; }
        }

        public LevelCameraController CameraLevelController
        {
            get { return _cameraLevelController; }
        }

        public BlocksFactory BlocksFactory
        {
            get { return _blocksFactory; }
        }
    }
}