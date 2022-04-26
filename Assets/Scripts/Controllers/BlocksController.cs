using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
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

        private List<DestroyableBlockView> _currentBlocks = new List<DestroyableBlockView>();
        private List<BlockData> _currentBlocksData = new List<BlockData>();
        private Vector3 _centerPoint;
        private DestroyableBlockView _currentHittingBlock;

        public void Init(CracksBlock cracksBlockPrefab, Transform levelCenterTransform, Transform levelParent)
        {
            _levelCenterTransform = levelCenterTransform;
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
            parent.GetComponentsInChildren(parent, _currentBlocks);
            
            foreach (DestroyableBlockView block in _currentBlocks)
            {
                SubscribeBlockView(block);
            }
            
            _centerPoint = FindCenterPoint(_currentBlocks);
            _levelCenterTransform.position = _centerPoint;
        }

        private void OnHit(DestroyableBlockView blockView)
        {
            OnBlockHit(blockView);
            onBlockHit?.Invoke(blockView);
        }

        private void OnBlockDestroy(DestroyableBlockView block)
        {
            onBlockDestroy?.Invoke(block);
            _currentBlocks.Remove(block);
            _currentBlocksData.Remove(block.DestroyableBlockData);
            
            _centerPoint = FindCenterPoint(_currentBlocks);
            if (_cracksBlocksPool != null)
            {
                _cracksBlocksPool.ReleaseObject(_blockToCracks[block]);
                _blockToCracks.Remove(block);
            }
            block.DestroyBlock();
            _currentHittingBlock = null;
            CreateDroppedBlock(block);
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

        public DestroyableBlockView CreateBlock(Vector3Int position, DestroyableBlockMetaData destroyableBlockMetaData = null)
        {
            DestroyableBlockView blockView =
                _blocksFactory.GetDestroyableBlockView(position, destroyableBlockMetaData, _levelCenterTransform);
            
            blockView.SetPosition((Vector3)position * _blocksFactory.BlockSize);
            _currentBlocks.Add(blockView);
            _currentBlocksData.Add(blockView.DestroyableBlockData);
            
            SubscribeBlockView(blockView);
            return blockView;
        }

        private void SubscribeBlockView(DestroyableBlockView blockView)
        {
            blockView.onBlockDestroy += OnBlockDestroy;
            blockView.onHit += OnHit;
        }

        public LevelData GetLevelData()
        {
            return new LevelData(_currentBlocksData);
        }

        public void DestroyBLock(DestroyableBlockView blockToDestroy)
        {
            OnBlockDestroy(blockToDestroy);
        }
    }
}