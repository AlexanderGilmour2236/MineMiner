using System;
using UnityEngine;

namespace MineMiner
{
    [ExecuteInEditMode]
    public class DestroyableBlockView : BlockView
    {
        [SerializeField] private DestroyableBlockMetaData destroyableBlockMetaData;
        
        private DestroyableBlockData _destroyableBlockData;

        public event Action<DestroyableBlockView> onHit;
        public event Action<DestroyableBlockView> onBlockDestroy;

        private void Start()
        {
            SetMetaData(blockMetaData);
        }

        public override void SetData(BlockData blockData)
        {
            base.SetData(blockData);
            _destroyableBlockData = (DestroyableBlockData)blockData;
        }

        public override void SetMetaData(BlockMetaData blockMetaData)
        {
            base.SetMetaData(blockMetaData);
            destroyableBlockMetaData = (DestroyableBlockMetaData)blockMetaData;
        }

        // TODO: get this 'return true' shit out of here you lame fat fuck
        /// <summary>
        /// Applies damage to block, returns true if block destroyed
        /// </summary>
        public bool Hit(float damage)
        {
            if (blockMetaData == null)
            {
                DestroyBlock();
                return true;
            }

            if (_destroyableBlockData.Hit(damage))
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
        }

        public void DestroyBlock()
        {
            Destroy(gameObject);
        }

        public float GetNormalizedValue()
        {
            return StrengthLeft / destroyableBlockMetaData.Strength;
        }

        public float StrengthLeft
        {
            get { return _destroyableBlockData.StrengthLeft; }
        }

        public DestroyableBlockMetaData DestroyableBlockMetaData
        {
            get { return destroyableBlockMetaData; }
        }

        public DestroyableBlockData DestroyableBlockData => _destroyableBlockData;
    }
}