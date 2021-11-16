using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Block
{
    [ExecuteInEditMode]
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private BlockData blockData = null;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private ParticleSystem destroyParticles;
        
        private bool _isPointerDown;
        private float _strengthLeft;
        private BlockData _lastBlockData = null;
        
        public event Action<BlockView> BlockDestroy;

        private void Start()
        {
            SetData(blockData);
        }

        private void Update()
        {
            if (blockData != _lastBlockData)
            {
                if (blockData != null && meshRenderer != null)
                {
                    meshRenderer.material = blockData.Material;
                }
            }
        }

        public void SetData(BlockData blockData)
        {
            this.blockData = blockData;
            _strengthLeft = this.blockData.Strength;
            meshRenderer.material = blockData.Material;
        }

        public bool Hit(float damage)
        {
            if (blockData == null)
            {
                DestroyBlock();
                return true;
            }
            _strengthLeft -= damage;
            
            if (_strengthLeft <= 0)
            {
                BlockDestroy?.Invoke(this);
                DestroyBlock();
                return true;
            }

            return false;
        }

        public void UndoHit()
        {
//            _strengthLeft = blockData.Strength;
        }

        private void DestroyBlock()
        {
            Destroy(gameObject);
        }

        public float GetNormalizedValue()
        {
            return StrengthLeft / Data.Strength;
        }
        
        public BlockData Data
        {
            get { return blockData; }
        }
        
        public float StrengthLeft
        {
            get { return _strengthLeft; }
        }
    }

}
