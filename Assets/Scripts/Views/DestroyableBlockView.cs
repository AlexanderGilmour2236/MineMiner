using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MineMiner
{
    public class DestroyableBlockView : BlockView, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private DestroyableBlockData destroyableBlockData;
        
        private bool _isPointerDown;
        private float _strengthLeft;

        public event Action<DestroyableBlockView> onPointerDown;
        public event Action<DestroyableBlockView> onBlockDestroy;

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
        }

        public void SetData(DestroyableBlockData blockData)
        {
            base.SetData(blockData);
            destroyableBlockData = blockData;
            _strengthLeft = destroyableBlockData.Strength;
        }

        public override void SetData(BlockData blockData)
        {
            base.SetData(blockData);
            destroyableBlockData = blockData as DestroyableBlockData;
            if (destroyableBlockData != null)
            {
                SetData(destroyableBlockData);
            }
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
            return StrengthLeft / destroyableBlockData.Strength;
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

        public float StrengthLeft
        {
            get { return _strengthLeft; }
        }

        public DestroyableBlockData DestroyableBlockData
        {
            get { return destroyableBlockData; }
        }
    }
}