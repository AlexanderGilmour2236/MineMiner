using System;
using System.Collections.Generic;
using Game.Block;
using UnityEngine;

namespace Game.Controllers
{
    public class BlocksController : MonoBehaviour
    {
        [SerializeField] private CracksBlock cracksBlock;
        [SerializeField] private Renderer renderer;
        [SerializeField] private Transform levelCenterTransform;
        
        public static string BlockTag = "Block";
        
        public event Action<BlockView> BlockDestroy;

        private BlockView _currentBlockView;
        private bool _undoRaycastHit;
        private bool _isRaycastingInCurrentFrame;

        private List<BlockView> _currentBlocks = new List<BlockView>();

        public void SetAllBlocks(Transform parent)
        {
            parent.GetComponentsInChildren(parent, _currentBlocks);
            foreach (BlockView block in _currentBlocks)
            {
                block.BlockDestroy += OnBlockDestroy;
            }
            
            levelCenterTransform.position = FindCenterPoint(_currentBlocks);
        }

        private void OnBlockDestroy(BlockView block)
        {
            BlockDestroy?.Invoke(block);
            _currentBlocks.Remove(block);
            levelCenterTransform.position = FindCenterPoint(_currentBlocks);
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

        private void LateUpdate()
        {
            if (!_isRaycastingInCurrentFrame)
            {
                if (_currentBlockView != null)
                {
                    _undoRaycastHit = true;
                }
            }

            if (_undoRaycastHit)
            {
                _currentBlockView.UndoHit();
                _currentBlockView = null;
            }
            
            if (_currentBlockView == null)
            {
                cracksBlock.gameObject.SetActive(false);
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
                        
                    if (_currentBlockView != null)
                    {
                        if (newBlockView != _currentBlockView)
                        {
                            _currentBlockView.UndoHit();
                        }
                    }

                    _currentBlockView = newBlockView;
                    _currentBlockView.Hit(Time.deltaTime);
                    cracksBlock.transform.position = _currentBlockView.transform.position;
                    cracksBlock.SetNormalizedValue(_currentBlockView.GetNormalizedValue());
                    cracksBlock.gameObject.SetActive(true);
                    return true;
                }
            }
            else
            {
                if (_currentBlockView != null)
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