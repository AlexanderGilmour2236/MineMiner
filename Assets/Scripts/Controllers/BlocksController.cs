using System;
using System.Collections.Generic;
using Game.Block;
using Misc;
using UnityEngine;

namespace Game.Controllers
{
    public class BlocksController : MonoBehaviour
    {
        [SerializeField] private CracksBlock cracksBlockPrefab;
        [SerializeField] private Transform levelCenterTransform;
        
        public static string BlockTag = "Block";

        public event Action<BlockView> BlockDestroy;

        private BlockView _hittingBlockView;
        private bool _undoRaycastHit;
        private bool _isRaycastingInCurrentFrame;
        private MonoPool<CracksBlock> _cracksBlocksPool;
        private Dictionary<BlockView, CracksBlock> _blockToCracks = new Dictionary<BlockView, CracksBlock>();

        private List<BlockView> _currentBlocks = new List<BlockView>();
        private Vector3 _centerPoint;
        public void Init()
        {
            _cracksBlocksPool = new MonoPool<CracksBlock>(transform, cracksBlockPrefab);
        }
        
        public void SetAllBlocks(Transform parent)
        {
            parent.GetComponentsInChildren(parent, _currentBlocks);
            foreach (BlockView block in _currentBlocks)
            {
                block.BlockDestroy += OnBlockDestroy;
            }
            
            _centerPoint = FindCenterPoint(_currentBlocks);
            levelCenterTransform.position = _centerPoint;
        }

        private void OnBlockDestroy(BlockView block)
        {
            _undoRaycastHit = true;
            BlockDestroy?.Invoke(block);
            _currentBlocks.Remove(block);
            _centerPoint = FindCenterPoint(_currentBlocks);
            _cracksBlocksPool.ReleaseObject(_blockToCracks[block]);
            _blockToCracks.Remove(block);
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

        private void Update()
        {
            levelCenterTransform.position = Vector3.Lerp(levelCenterTransform.position, _centerPoint, 0.1f);
        }

        private void LateUpdate()
        {
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

        public bool TryHitBlock(Ray ray)
        {
            _isRaycastingInCurrentFrame = true;
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                if (objectHit.CompareTag(BlockTag))
                {
                    var newBlockView = objectHit.GetComponent<BlockView>();
                        
                    _hittingBlockView = newBlockView;
                    if (_hittingBlockView.Hit(Time.deltaTime))
                    {
                        return false;
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
                    return true;
                }
            }
            else
            {
                if (_hittingBlockView != null)
                {
                    _undoRaycastHit = true;
                }
            }

            return false;
        }
        
        public Transform LevelCenterTransform
        {
            get { return levelCenterTransform; }
        }
    }
}