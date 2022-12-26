using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MineMiner
{
    public class MapEditorNavigator : SceneNavigator
    {
        private BlocksFactory _blocksFactory;
        private MapEditorSceneAccessor _mapEditorSceneAccessor;
        private MapEditorCameraController _mapEditorCameraController;
        private BlocksController _blocksController;
        private LevelGenerator _levelGenerator;
        
        private string _saveLoadPath;
        private string _filePath;
        private MapEditorUI _mapEditorUI;
        private List<BlockView> _blocksList = new List<BlockView>();
        private DestroyableBlockView _currentSampleBlockView;
        private DestroyableBlockMetaData _currentBlockMetaData;
        private List<DestroyableBlockMetaData> _allBlocksMetaData;

        public MapEditorNavigator(string sceneName) : base(sceneName)
        {
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            base.OnSceneLoaded(scene, loadSceneMode);
            _mapEditorSceneAccessor = Object.FindObjectOfType<MapEditorSceneAccessor>();
            _mapEditorCameraController = _mapEditorSceneAccessor.MapEditorCameraController;
            _blocksFactory = _mapEditorSceneAccessor.BlocksFactory;
            _blocksController = new BlocksController(_blocksFactory, new Player() { Damage = 999 });
            _mapEditorUI = _mapEditorSceneAccessor.MapEditorUI;
            _saveLoadPath = $"{Application.dataPath}/Resources/Levels/";
            _mapEditorUI.NewLevelButton.onClick.AddListener(OnNewLevelButtonClick);
            _mapEditorUI.SaveLevelButton.onClick.AddListener(OnSaveLevel);
            _mapEditorUI.LoadLevelButton.onClick.AddListener(OnLoadLevelButtonClick);
            _mapEditorUI.GenerateLevelButton.onClick.AddListener(OnGenerateButtonClick);
            _blocksController.Init(null, _mapEditorSceneAccessor.LevelStartPoint, _mapEditorSceneAccessor.LevelStartPoint);
            _currentSampleBlockView = _mapEditorSceneAccessor.CurrentSampleBlockView;
            _allBlocksMetaData =  new List<DestroyableBlockMetaData>(_blocksFactory.GetAllBlocksData());
            SetCurrentBlockData(_blocksFactory.DefaultBlockMetaData);

            InitControllers();
        }

        private void OnGenerateButtonClick()
        {
            _levelGenerator.SetNoiseRenderer(_mapEditorSceneAccessor._noizeImage);
            StartEditingLevel(_filePath, StartEditingLevelMode.GenerateLevel);
        }

        private void SetCurrentBlockData(DestroyableBlockMetaData destroyableBlockMetaData)
        {
            _currentBlockMetaData = destroyableBlockMetaData;
            _currentSampleBlockView.SetMetaData(destroyableBlockMetaData);
        }

        private void OnLoadLevelButtonClick()
        {
#if UNITY_EDITOR
            _filePath = EditorUtility.OpenFilePanel("Open level for editing", _saveLoadPath, "json");
            StartEditingLevel(_filePath, StartEditingLevelMode.LoadLevel);    
#endif
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
                SetPrevCurrentBlockData();
            }
            
            if (Input.mouseScrollDelta.y < 0)
            {
                SetNextCurrentBlockData();
            }
        }

        private void SetPrevCurrentBlockData()
        {
            int nextIndex = _allBlocksMetaData.IndexOf(_currentBlockMetaData) - 1;
            if (nextIndex < 0)
            {
                nextIndex = _allBlocksMetaData.Count - 1;
            }

            _currentBlockMetaData = _allBlocksMetaData[nextIndex];
            _currentSampleBlockView.SetMetaData(_currentBlockMetaData);
        }

        private void SetNextCurrentBlockData()
        {
            int nextIndex = _allBlocksMetaData.IndexOf(_currentBlockMetaData) + 1;
            if (nextIndex >= _allBlocksMetaData.Count)
            {
                nextIndex = 0;
            }
            _currentBlockMetaData = _allBlocksMetaData[nextIndex];
            _currentSampleBlockView.SetMetaData(_currentBlockMetaData);
        }

        private void OnNewLevelButtonClick()
        {
#if UNITY_EDITOR
            _filePath = EditorUtility.SaveFilePanel("Create file for a new Level", 
                _saveLoadPath, "Level00.json", ".json");
            StartEditingLevel(_filePath, StartEditingLevelMode.NewLevel);
#endif
        }

        private void StartEditingLevel(string filePath, StartEditingLevelMode startEditingLevelMode)
        {
            ClearLastLevel();
            switch (startEditingLevelMode)
            {
                case StartEditingLevelMode.GenerateLevel:
                    _levelGenerator.GenerateLevel(Random.Range(3, 6),Random.Range(5,10), Random.Range(3, 7));
                    _blocksList.AddRange(_blocksController.CurrentBlocks);
                    break;
                case StartEditingLevelMode.NewLevel:
                    CreateFirstBlock();
                    break;
                case StartEditingLevelMode.LoadLevel:
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
