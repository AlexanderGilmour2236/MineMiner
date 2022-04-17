using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MineMiner
{
    [ExecuteInEditMode]
    public class DestroyableBlockView : BlockView, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private DestroyableBlockData _destroyableBlockData;
        
        private bool _isPointerDown;
        private float _strengthLeft;

        public event Action<DestroyableBlockView> onHit;
        public event Action<DestroyableBlockView> onLeftPointerDown;
        public event Action<DestroyableBlockView> onRightPointerDown;
        public event Action<DestroyableBlockView> onBlockDestroy;

        private void Start()
        {
            SetData(_blockData);
        }

        public override void Update()
        {
            base.Update();
            if (_isPointerDown)
            {
                onHit?.Invoke(this);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isPointerDown = false;
            }
        }

        public void SetData(DestroyableBlockData blockData)
        {
            base.SetData(blockData);
            _destroyableBlockData = blockData;
            _strengthLeft = _destroyableBlockData.Strength;
        }

        public override void SetData(BlockData blockData)
        {
            base.SetData(blockData);
            _destroyableBlockData = blockData as DestroyableBlockData;
            if (_destroyableBlockData != null)
            {
                SetData(_destroyableBlockData);
            }
        }

        /// <summary>
        /// Applies damage to block, returns true if block destroyed
        /// </summary>
        public bool Hit(float damage)
        {
            if (_blockData == null)
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
            print("UndoHit");
            _isPointerDown = false;
        }

        public void DestroyBlock()
        {
            Destroy(gameObject);
        }

        public float GetNormalizedValue()
        {
            return StrengthLeft / _destroyableBlockData.Strength;
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButton(0))
            {
                _isPointerDown = true;
            }
        }

        private void OnMouseExit()
        {
            _isPointerDown = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerDown = true;
            if (Input.GetMouseButtonDown(0))
            {
                onLeftPointerDown?.Invoke(this);
            }
            if (Input.GetMouseButtonDown(1))
            {
                onRightPointerDown?.Invoke(this);
            }
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
            get { return _destroyableBlockData; }
        }
    }
}