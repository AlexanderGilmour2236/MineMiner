using System.Collections.Generic;
using SimpleJSON;
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
        [Inject] private BlocksController _blocksController;
        
        private string _saveLoadPath;
        private string _filePath;
        private MapEditorUI _mapEditorUI;
        private List<BlockView> _blocksList = new List<BlockView>();
        private DestroyableBlockMetaData blockMetaData;

        public override void Go()
        {
            base.Go();

            blockMetaData = _blocksFactory.DefaultBlockMetaData;
            _mapEditorUI = _mapEditorSceneAccessor.MapEditorUI;
            _saveLoadPath = $"{Application.dataPath}/Resources/Levels/";
            _mapEditorUI.NewLevelButton.onClick.AddListener(OnNewLevelButtonClick);
            _mapEditorUI.SaveLevelButton.onClick.AddListener(OnSaveLevel);
            _mapEditorUI.LoadLevelButton.onClick.AddListener(OnLoadLevelButtonClick);
            _blocksController.Init(null, _mapEditorSceneAccessor.LevelStartPoint, _mapEditorSceneAccessor.LevelStartPoint);
            
            InitControllers();
        }

        private void OnLoadLevelButtonClick()
        {
            _filePath = EditorUtility.OpenFilePanel("Open level for editing", _saveLoadPath, "json");
            StartEditingLevel(_filePath, false);
        }

        private void OnSaveLevel()
        {
            SaveLoadController.SaveObject(_filePath, _blocksController.GetLevelData());
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

            DestroyableBlockView hitBlockView = null;
            if (Input.GetMouseButtonDown(0))
            {
                hitBlockView = GetBlockFromCameraRay();
                if (hitBlockView != null)
                {
                    DestroyBlock(hitBlockView);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                hitBlockView = GetBlockFromCameraRay();
                if (hitBlockView != null)
                {
                    CreateBlockOnBlock(hitBlockView);
                }
            }
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
                LoadLevel(filePath);
            }
        }

        private void CreateFirstBlock()
        {
            _blocksController.CreateBlock(new Vector3Int(0,0,0));
        }

        private DestroyableBlockView GetBlockFromCameraRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit? hit = GetCameraRayHit();
            if (hit != null)
            {
                if (hit.Value.collider.CompareTag(TagManager.BlockTag))
                {
                    return hit.Value.collider.GetComponent<DestroyableBlockView>();
                }
            }
            return null;
        }

        private RaycastHit? GetCameraRayHit()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit;
            }

            return null;
        }

        private void DestroyBlock(DestroyableBlockView blockToDestroy)
        {
            if (_blocksList.Count > 1)
            {
                _blocksController.DestroyBLock(blockToDestroy);
                _blocksList.Remove(blockToDestroy);
            }
        }
        
        private void CreateBlockOnBlock(DestroyableBlockView hitBlockView)
        {
            RaycastHit? raycastHit = GetCameraRayHit();
            Vector3Int newBlockPosition = hitBlockView.DestroyableBlockData.Position + new Vector3Int((int)raycastHit.Value.normal.x, (int)raycastHit.Value.normal.y, (int)raycastHit.Value.normal.z);

            CreateBlock(newBlockPosition);
        }

        private void CreateBlock(Vector3Int newBlockPosition)
        {
            DestroyableBlockView newBlockView = _blocksController.CreateBlock(newBlockPosition, blockMetaData);
            _blocksList.Add(newBlockView);
        }

        private void LoadLevel(string filePath)
        {
            LevelData levelData = SaveLoadController.LoadObject(filePath, (json) => new LevelData(json));
            foreach (BlockData blockData in levelData.BlockDatas)
            {
                blockData.BlockMetaData = _blocksFactory.GetBlockMetaData(blockData.BlockId); 
                if (blockData.BlockDataType == BlockDataType.Destroyable)
                {
                    DestroyableBlockData destroyableBlockData = (DestroyableBlockData) blockData;
                    CreateBlock(destroyableBlockData.Position);
                }
            }
        }
    }
}
