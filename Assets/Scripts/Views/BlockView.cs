using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace MineMiner
{
    [ExecuteInEditMode]
    public class BlockView : MonoBehaviour
    {
        [SerializeField] protected BlockData _blockData;
        [SerializeField] protected BlockRendererStrategy _blockRendererStrategy;
        [SerializeField] protected BlockMovementStrategy _blockMovementStrategy;
        
        private DestroyableBlockData _lastBlockData = null;

        public virtual void SetData(BlockData blockData)
        {
            _blockData = blockData;
            _blockRendererStrategy.SetMaterial(blockData.Material);
        }

        public virtual void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        
        public virtual BlockData Data
        {
            get { return _blockData; }
        }

        public virtual void Update()
        {
            if(_blockData != _lastBlockData)
            {
                if (_blockData != null && _blockRendererStrategy != null)
                {
                    _blockRendererStrategy.SetData(_blockData);
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
