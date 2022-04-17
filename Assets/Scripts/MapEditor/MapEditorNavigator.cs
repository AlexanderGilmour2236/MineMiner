using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MapEditorNavigator : Navigator
    {
        [Inject] private BlocksFactory _blocksFactory;
        [Inject] private MapEditorSceneAccessor _mapEditorSceneAccessor;
        [Inject] private MapEditorCameraController _mapEditorCameraController;

        private string _saveLoadPath;
        private string _filePath;
        private MapEditorUI _mapEditorUI;
        private List<BlockView> _blocksList = new List<BlockView>();

        public override void Go()
        {
            base.Go();
            
            _mapEditorUI = _mapEditorSceneAccessor.MapEditorUI;
            _saveLoadPath = $"{Application.dataPath}/Resources/Levels/";
            _mapEditorUI.NewLevelButton.onClick.AddListener(OnNewLevelButtonClick);

            InitControllers();
        }

        private void InitControllers()
        {
            _mapEditorCameraController.Init();
            _isControllersInitiated = true;
        }

        public override void Tick()
        {
            base.Tick();
            if (!_isControllersInitiated)
            {
                return;
            }

            _mapEditorCameraController.Tick();
        }

        private void OnNewLevelButtonClick()
        {
            _filePath = EditorUtility.SaveFilePanel("Create file for a new Level", 
                _saveLoadPath, "Level00.json", ".json");
            StartEditingLevel(_filePath, true);
        }

        private void StartEditingLevel(string filePath, bool newLevel)
        {
            if (newLevel)
            {
                CreateFirstBlock();
            }
            else
            {
                LoadMap(filePath);
            }
        }

        private void CreateFirstBlock()
        {
            CreateBlock();
        }

        private void CreateBlock(DestroyableBlockData destroyableBlockData = null)
        {
            DestroyableBlockView blockView =
                _blocksFactory.GetDestroyableBlockView(destroyableBlockData, _mapEditorSceneAccessor.LevelStartPoint);
            // blockView.SetPosition((Vector3)destroyableBlockData.Position * _blocksFactory.BlockSize);
            _blocksList.Add(blockView);
            
            SubscribeBlockView(blockView);
        }

        private void SubscribeBlockView(DestroyableBlockView blockView)
        {
            blockView.onLeftPointerDown += DestroyBlock;
            blockView.onRightPointerDown += CreateBlockOnBlock;
        }

        private void CreateBlockOnBlock(DestroyableBlockView createOnBlockView)
        {
            Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void DestroyBlock(DestroyableBlockView blockToDestroy)
        {
            blockToDestroy.DestroyBlock();
            _blocksList.Remove(blockToDestroy);
        }

        private void LoadMap(string filePath)
        {
        }
    }
}
