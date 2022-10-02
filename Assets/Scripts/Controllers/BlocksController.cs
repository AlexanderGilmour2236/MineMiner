using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MineMiner
{
    public class BlocksController
    {
        [Inject] private BlocksFactory _blocksFactory;
        
        private Transform _levelCenterTransform;
        
        public event Action<DestroyableBlockView> onBlockDestroy;
        public event Action<DestroyableBlockView> onBlockHit;

        private bool _undoRaycastHit;
        private bool _isHittingBlockInCurrentFrame;
        private MonoPool<CracksBlock> _cracksBlocksPool;
        private Dictionary<BlockView, CracksBlock> _blockToCracks = new Dictionary<BlockView, CracksBlock>();

        private List<DestroyableBlockView> _currentCurrentBlocks = new List<DestroyableBlockView>();
        private List<BlockData> _currentBlocksData = new List<BlockData>();
        private Vector3 _centerPoint;
        private DestroyableBlockView _currentHittingBlock;
        private Transform _levelParent;

        public void Init(CracksBlock cracksBlockPrefab, Transform levelCenterTransform, Transform levelParent)
        {
            _levelCenterTransform = levelCenterTransform;
            _levelParent = levelParent;
            if (cracksBlockPrefab != null)
            {
                _cracksBlocksPool = new MonoPool<CracksBlock>(levelParent, cracksBlockPrefab);
            }
        }

        public void Tick()
        {
            _levelCenterTransform.position = Vector3.Lerp(_levelCenterTransform.position, _centerPoint, 0.1f);
            
            if (!_isHittingBlockInCurrentFrame)
            {
                if (_currentHittingBlock != null)
                {
                    _undoRaycastHit = true;
                }
            }

            if (_undoRaycastHit)
            {
                _currentHittingBlock = null;
            }
            
            _isHittingBlockInCurrentFrame = false;
            _undoRaycastHit = false;
        }

        public void SetAllBlocksFromLevelGameObjects(Transform parent)
        {
            parent.GetComponentsInChildren(parent, _currentCurrentBlocks);
            
            foreach (DestroyableBlockView block in _currentCurrentBlocks)
            {
                subscribeBlockView(block);
            }
            
            _centerPoint = FindCenterPoint(_currentCurrentBlocks);
            _levelCenterTransform.position = _centerPoint;
        }
        
        public void SetBlocks(DestroyableBlockView[] getPlayerLevelBlocks)
        {
            _currentCurrentBlocks.Clear();
            _currentCurrentBlocks.AddRange(getPlayerLevelBlocks);
            
            foreach (DestroyableBlockView block in getPlayerLevelBlocks)
            {
                subscribeBlockView(block);
            }
        }

        public void OnHit(DestroyableBlockView blockView)
        {
            OnBlockHit(blockView);
            onBlockHit?.Invoke(blockView);
        }

        private void OnBlockDestroy(DestroyableBlockView block)
        {
            DestroyBlock(block);
            CreateDroppedBlock(block);
        }

        public void DestroyBlock(DestroyableBlockView block)
        {
            onBlockDestroy?.Invoke(block);
            _currentCurrentBlocks.Remove(block);
            _currentBlocksData.Remove(block.DestroyableBlockData);

            if (_cracksBlocksPool != null)
            {
                _cracksBlocksPool.ReleaseObject(_blockToCracks[block]);
                _blockToCracks.Remove(block);
            }

            block.DestroyBlock();
            _currentHittingBlock = null;
        }

        private void CreateDroppedBlock(DestroyableBlockView block)
        {
            BlockView[] droppedBlockViews = _blocksFactory.GetDroppedBlockViews(block.DestroyableBlockMetaData);
            foreach (BlockView droppedBlockView in droppedBlockViews)
            {
                droppedBlockView.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(0.5f, 1), Random.Range(-1, 1)));
                droppedBlockView.transform.position = block.transform.position;
                droppedBlockView.AddRotation(new Vector3(Random.Range(0, 180), Random.Range(0, 180),
                    Random.Range(0, 180))); 
            }
        }

        Vector3 FindCenterPoint(List<DestroyableBlockView> blocks)
        {
            Vector3 blockCenter  = Vector3.zero;
            float count = 0;

            foreach (DestroyableBlockView block in blocks){
                blockCenter += block.transform.position;
                count++;
            }
            
            Vector3 centerPoint = blockCenter / count;

            return centerPoint;
        }

        public void OnBlockHit(DestroyableBlockView blockView)
        {
            if (_currentHittingBlock != null)
            {
                _currentHittingBlock.UndoHit();
            }
            _currentHittingBlock = blockView;

            if (_cracksBlocksPool != null)
            {
                CracksBlock cracksBlock;
                if (!_blockToCracks.ContainsKey(_currentHittingBlock))
                {
                    cracksBlock = _cracksBlocksPool.GetObject();
                    cracksBlock.transform.position = _currentHittingBlock.transform.position;
                }
                else
                {
                    cracksBlock = _blockToCracks[_currentHittingBlock];
                }
            
                cracksBlock.SetNormalizedValue(_currentHittingBlock.GetNormalizedValue());

                _blockToCracks[_currentHittingBlock] = cracksBlock;   
            }
        }
        
        public Transform LevelCenterTransform
        {
            get { return _levelCenterTransform; }
        }

        public List<DestroyableBlockView> CurrentBlocks
        {
            get
            {
                return _currentCurrentBlocks;
            }
        }

        public DestroyableBlockView CreateBlock(Vector3Int position, DestroyableBlockMetaData destroyableBlockMetaData = null)
        {
            DestroyableBlockView blockView =
                _blocksFactory.GetDestroyableBlockView(position, destroyableBlockMetaData, _levelParent);
            
            blockView.SetPosition((Vector3)position * _blocksFactory.BlockSize);
            _currentCurrentBlocks.Add(blockView);
            _currentBlocksData.Add(blockView.DestroyableBlockData);
            
            subscribeBlockView(blockView);
            
            return blockView;
        }

        public void SetCenterPoint()
        {
            _centerPoint = FindCenterPoint(_currentCurrentBlocks);
            _levelCenterTransform.position = _centerPoint;
        }
        
        private void subscribeBlockView(DestroyableBlockView blockView)
        {
            blockView.onBlockDestroy += OnBlockDestroy;
            blockView.onHit += OnHit;
        }

        public LevelData GetLevelData()
        {
            return new LevelData(_currentBlocksData);
        }

        public void Clear()
        {
            DestroyableBlockView[] destroyableBlockViews = _currentCurrentBlocks.ToArray();
            foreach (DestroyableBlockView destroyableBlockView in destroyableBlockViews)
            {
                DestroyBlock(destroyableBlockView);
            }
        }
    }
}