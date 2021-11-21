using System;
using System.Collections.Generic;
using UnityEngine;

namespace MineMiner
{
    public class BlocksController
    {
        private CracksBlock _cracksBlockPrefab;
        private Transform _levelCenterTransform;
        
        public static string BlockTag = "Block";

        public event Action<BlockView> onBlockDestroy;
        public event Action<BlockView> onBlockHit;

        private BlockView _hittingBlockView;
        private bool _undoRaycastHit;
        private bool _isRaycastingInCurrentFrame;
        private MonoPool<CracksBlock> _cracksBlocksPool;
        private Dictionary<BlockView, CracksBlock> _blockToCracks = new Dictionary<BlockView, CracksBlock>();

        private List<BlockView> _currentBlocks = new List<BlockView>();
        private Vector3 _centerPoint;
        private BlockView _currentHittingBlock;

        public void Init(CracksBlock cracksBlockPrefab, Transform levelCenterTransform, Transform levelParent)
        {
            _levelCenterTransform = levelCenterTransform;
            _cracksBlocksPool = new MonoPool<CracksBlock>(levelParent, cracksBlockPrefab);
        }
        
        public void SetAllBlocks(Transform parent)
        {
            parent.GetComponentsInChildren(parent, _currentBlocks);
            foreach (BlockView block in _currentBlocks)
            {
                block.onBlockDestroy += OnOnBlockDestroy;
                block.onPointerDown += OnPointerDown;
            }
            
            _centerPoint = FindCenterPoint(_currentBlocks);
            _levelCenterTransform.position = _centerPoint;
        }

        private void OnPointerDown(BlockView blockView)
        {
            OnBlockHit(blockView);
            onBlockHit?.Invoke(blockView);
        }

        private void OnOnBlockDestroy(BlockView block)
        {
            _undoRaycastHit = true;
            onBlockDestroy?.Invoke(block);
            _currentBlocks.Remove(block);
            _centerPoint = FindCenterPoint(_currentBlocks);
            _cracksBlocksPool.ReleaseObject(_blockToCracks[block]);
            _blockToCracks.Remove(block);
            block.DestroyBlock();
        }

        Vector3 FindCenterPoint(List<BlockView> blocks)
        {
            Vector3 blockCenter  = Vector3.zero;
            float count = 0;

            foreach (BlockView block in blocks){
                blockCenter += block.transform.position;
                count++;
            }
            
            Vector3 centerPoint = blockCenter / count;

            return centerPoint;
        }

        public void Tick()
        {
            _levelCenterTransform.position = Vector3.Lerp(_levelCenterTransform.position, _centerPoint, 0.1f);
            
            if (!_isRaycastingInCurrentFrame)
            {
                if (_hittingBlockView != null)
                {
                    _undoRaycastHit = true;
                }
            }

            if (_undoRaycastHit)
            {
                _hittingBlockView = null;
            }
            
            _isRaycastingInCurrentFrame = false;
            _undoRaycastHit = false;
        }

        public void OnBlockHit(BlockView blockView)
        {
            if (_currentHittingBlock != null)
            {
                _currentHittingBlock.UndoHit();
            }
            _currentHittingBlock = blockView;
            _isRaycastingInCurrentFrame = true;
            
            Transform objectHit = blockView.transform;
            BlockView newBlockView = objectHit.GetComponent<BlockView>();
                
            _hittingBlockView = newBlockView;
            if (_hittingBlockView.Hit(Time.deltaTime * App.Instance().Player.Damage))
            {
                return;
            }

            CracksBlock cracksBlock;
            if (!_blockToCracks.ContainsKey(_hittingBlockView))
            {
                cracksBlock = _cracksBlocksPool.GetObject();
                cracksBlock.transform.position = _hittingBlockView.transform.position;
            }
            else
            {
                cracksBlock = _blockToCracks[_hittingBlockView];
            }
            
            cracksBlock.SetNormalizedValue(_hittingBlockView.GetNormalizedValue());

            _blockToCracks[_hittingBlockView] = cracksBlock;
            return;
        }
        
        public Transform LevelCenterTransform
        {
            get { return _levelCenterTransform; }
        }
    }
}