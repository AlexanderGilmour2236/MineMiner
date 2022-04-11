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
        
        public event Action<BlockView> onBlockDestroy;
        public event Action<BlockView> onBlockHit;

        private bool _undoRaycastHit;
        private bool _isHittingBlockInCurrentFrame;
        private MonoPool<CracksBlock> _cracksBlocksPool;
        private Dictionary<BlockView, CracksBlock> _blockToCracks = new Dictionary<BlockView, CracksBlock>();

        private List<DestroyableBlockView> _currentBlocks = new List<DestroyableBlockView>();
        private Vector3 _centerPoint;
        private DestroyableBlockView _currentHittingBlock;

        public void Init(CracksBlock cracksBlockPrefab, Transform levelCenterTransform, Transform levelParent)
        {
            _levelCenterTransform = levelCenterTransform;
            _cracksBlocksPool = new MonoPool<CracksBlock>(levelParent, cracksBlockPrefab);
            
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

        public void SetAllBlocks(Transform parent)
        {
            parent.GetComponentsInChildren(parent, _currentBlocks);
            foreach (DestroyableBlockView block in _currentBlocks)
            {
                block.onBlockDestroy += OnBlockDestroy;
                block.onPointerDown += OnPointerDown;
            }
            
            _centerPoint = FindCenterPoint(_currentBlocks);
            _levelCenterTransform.position = _centerPoint;
        }

        private void OnPointerDown(DestroyableBlockView blockView)
        {
            OnBlockHit(blockView);
            onBlockHit?.Invoke(blockView);
        }

        private void OnBlockDestroy(DestroyableBlockView block)
        {
//            _undoRaycastHit = true;
            onBlockDestroy?.Invoke(block);
            _currentBlocks.Remove(block);
            _centerPoint = FindCenterPoint(_currentBlocks);
            _cracksBlocksPool.ReleaseObject(_blockToCracks[block]);
            _blockToCracks.Remove(block);
            block.DestroyBlock();
            _currentHittingBlock = null;
            CreateDroppedBlock(block);
        }

        private void CreateDroppedBlock(DestroyableBlockView block)
        {
            BlockView[] droppedBlockViews = _blocksFactory.GetDroppedBlockView(block.DestroyableBlockData);
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
//            _isHittingBlockInCurrentFrame = true;
            
            _currentHittingBlock = blockView;
            
            if (_currentHittingBlock.Hit(Time.deltaTime * App.Instance().Player.Damage))
            {
                return;
            }

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
        
        public Transform LevelCenterTransform
        {
            get { return _levelCenterTransform; }
        }
    }
}