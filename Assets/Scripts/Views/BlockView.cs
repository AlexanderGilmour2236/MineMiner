using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace MineMiner
{
    [ExecuteInEditMode]
    public class BlockView : MonoBehaviour
    {
        [SerializeField] protected BlockMetaData blockMetaData;
        [SerializeField] protected BlockRendererStrategy _blockRendererStrategy;
        [SerializeField] protected BlockMovementStrategy _blockMovementStrategy;
        
        private BlockMetaData _lastBlockMetaData = null;
        protected BlockData _blockData;
            
        public virtual void SetMetaData(BlockMetaData blockMetaData)
        {
            this.blockMetaData = blockMetaData;
            _blockRendererStrategy.SetMaterial(blockMetaData.Material);
        }

        public virtual void SetData(BlockData blockData)
        {
            _blockData = blockData;
        }

        public virtual void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        
        public virtual BlockMetaData MetaData
        {
            get { return blockMetaData; }
        }

        public virtual void Update()
        {
            if(blockMetaData != _lastBlockMetaData)
            {
                if (blockMetaData != null && _blockRendererStrategy != null)
                {
                    _blockRendererStrategy.SetData(blockMetaData);
                }
            }
        }


        public void AddRotation(Vector3 rotation)
        {
            _blockMovementStrategy.AddTorque(rotation);
        }

        public void AddForce(Vector3 movement)
        {
            _blockMovementStrategy.AddForce(movement);
        }
    }

}
