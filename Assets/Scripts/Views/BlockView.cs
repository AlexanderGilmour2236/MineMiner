using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace MineMiner
{
    [ExecuteInEditMode]
    public class BlockView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private BlockData blockData = null;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private ParticleSystem destroyParticles;
        
        private bool _isPointerDown;
        private float _strengthLeft;
        private BlockData _lastBlockData = null;

        public event Action<BlockView> onPointerDown;
        public event Action<BlockView> onBlockDestroy;

        private void Start()
        {
            SetData(blockData);
        }

        private void Update()
        {
            if (_isPointerDown)
            {
                onPointerDown?.Invoke(this);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isPointerDown = false;
            }
            
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
                onBlockDestroy?.Invoke(this);
                DestroyBlock();
                return true;
            }

            return false;
        }

        public void UndoHit()
        {
            _isPointerDown = false;
        }

        public void DestroyBlock()
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

        private void OnMouseOver()
        {
            if (Input.GetMouseButton(0))
            {
                _isPointerDown = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }

}
