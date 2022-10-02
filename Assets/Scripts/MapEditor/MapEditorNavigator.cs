using System.Collections.Generic;
using ModestTree;
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
        [Inject] private LevelGenerator _levelGenerator;
        
        private string _saveLoadPath;
        private string _filePath;
        private MapEditorUI _mapEditorUI;
        private List<BlockView> _blocksList = new List<BlockView>();
        private DestroyableBlockView _currentSampleBlockView;
        private DestroyableBlockMetaData _currentBlockMetaData;
        private DestroyableBlockMetaData[] _allBlocksMetaData;

        public override void Go()
        {
            base.Go();

            _mapEditorUI = _mapEditorSceneAccessor.MapEditorUI;
            _saveLoadPath = $"{Application.dataPath}/Resources/Levels/";
            _mapEditorUI.NewLevelButton.onClick.AddListener(OnNewLevelButtonClick);
            _mapEditorUI.SaveLevelButton.onClick.AddListener(OnSaveLevel);
            _mapEditorUI.LoadLevelButton.onClick.AddListener(OnLoadLevelButtonClick);
            _mapEditorUI.GenerateLevelButton.onClick.AddListener(OnGenerateButtonClick);
            _blocksController.Init(null, _mapEditorSceneAccessor.LevelStartPoint, _mapEditorSceneAccessor.LevelStartPoint);
            _currentSampleBlockView = _mapEditorSceneAccessor.CurrentSampleBlockView;
            _allBlocksMetaData = _blocksFactory.GetAllBlocksData();
            setCurrentBlockData(_blocksFactory.DefaultBlockMetaData);

            InitControllers();
        }

        private void OnGenerateButtonClick()
        {
            _levelGenerator.SetNoiseRenderer(_mapEditorSceneAccessor.NoizeImage);
            StartEditingLevel(_filePath, StartEditingLevelMode.GENERATE_LEVEL);
        }

        private void setCurrentBlockData(DestroyableBlockMetaData destroyableBlockMetaData)
        {
            _currentBlockMetaData = destroyableBlockMetaData;
            _currentSampleBlockView.SetMetaData(destroyableBlockMetaData);
        }

        private void OnLoadLevelButtonClick()
        {
            _filePath = EditorUtility.OpenFilePanel("Open level for editing", _saveLoadPath, "json");
            StartEditingLevel(_filePath, StartEditingLevelMode.LOAD_LEVEL);
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

            if (Input.mouseScrollDelta.y > 0)
            {
                setPrevCurrentBlockData();
            }
            
            if (Input.mouseScrollDelta.y < 0)
            {
                setNextCurrentBlockData();
            }
        }

        private void setPrevCurrentBlockData()
        {
            int nextIndex = _allBlocksMetaData.IndexOf(_currentBlockMetaData) - 1;
            if (nextIndex < 0)
            {
                nextIndex = _allBlocksMetaData.Length - 1;
            }

            _currentBlockMetaData = _allBlocksMetaData[nextIndex];
            _currentSampleBlockView.SetMetaData(_currentBlockMetaData);
        }

        private void setNextCurrentBlockData()
        {
            int nextIndex = _allBlocksMetaData.IndexOf(_currentBlockMetaData) + 1;
            if (nextIndex >= _allBlocksMetaData.Length)
            {
                nextIndex = 0;
            }
            _currentBlockMetaData = _allBlocksMetaData[nextIndex];
            _currentSampleBlockView.SetMetaData(_currentBlockMetaData);
        }

        private void OnNewLevelButtonClick()
        {
            _filePath = EditorUtility.SaveFilePanel("Create file for a new Level", 
                _saveLoadPath, "Level00.json", ".json");
            StartEditingLevel(_filePath, StartEditingLevelMode.NEW_LEVEL);
        }

        private void StartEditingLevel(string filePath, StartEditingLevelMode startEditingLevelMode)
        {
            ClearLastLevel();
            switch (startEditingLevelMode)
            {
                case StartEditingLevelMode.GENERATE_LEVEL:
                    _levelGenerator.GenerateLevel(Random.Range(3, 6),Random.Range(5,10), Random.Range(3, 7));
                    
                    // _levelGenerator.GenerateLevel(20,20, 20);
                    _blocksList.AddRange(_blocksController.CurrentBlocks);
                    break;
                case StartEditingLevelMode.NEW_LEVEL:
                    CreateFirstBlock();
                    break;
                case StartEditingLevelMode.LOAD_LEVEL:
                    LoadLevel(filePath);
                    break;
            }
        }

        private void ClearLastLevel()
        {
            _blocksController.Clear();
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
                _blocksController.DestroyBlock(blockToDestroy);
                _blocksList.Remove(blockToDestroy);
            }
        }
        
        private void CreateBlockOnBlock(DestroyableBlockView hitBlockView)
        {
            RaycastHit? raycastHit = GetCameraRayHit();
            Vector3Int newBlockPosition = hitBlockView.DestroyableBlockData.Position + new Vector3Int((int)raycastHit.Value.normal.x, (int)raycastHit.Value.normal.y, (int)raycastHit.Value.normal.z);

            CreateBlock(newBlockPosition, _currentBlockMetaData);
        }

        private void CreateBlock(Vector3Int newBlockPosition, DestroyableBlockMetaData blockMetaData)
        {
            DestroyableBlockView newBlockView = _blocksController.CreateBlock(newBlockPosition, blockMetaData);
            _blocksList.Add(newBlockView);
        }

        private void LoadLevel(string filePath)
        {
            LevelData levelData = SaveLoadController.LoadObject(filePath, (json) => new LevelData(json));
            foreach (BlockData blockData in levelData.BlockDatas)
            {
                Debug.Log(blockData.BlockId);
                blockData.BlockMetaData = _blocksFactory.GetBlockMetaData(blockData.BlockId); 
                if (blockData.BlockDataType == BlockDataType.Destroyable)
                {
                    DestroyableBlockData destroyableBlockData = (DestroyableBlockData) blockData;
                    CreateBlock(destroyableBlockData.Position, (DestroyableBlockMetaData)destroyableBlockData.BlockMetaData);
                }
            }
        }
    }
}
