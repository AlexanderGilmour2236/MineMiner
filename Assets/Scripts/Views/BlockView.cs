using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace MineMiner
{
    [ExecuteInEditMode]
    public class BlockView : MonoBehaviour
    {
        [SerializeField] protected BlockData blockData;
        [SerializeField] protected BlockRendererStrategy blockRendererStrategy;
        [SerializeField] protected BlockRotationStrategy blockRotationStrategy;
        
        private DestroyableBlockData _lastBlockData = null;

        public virtual void SetData(BlockData blockData)
        {
            this.blockData = blockData;
            blockRendererStrategy.SetMaterial(blockData.Material);
        }
        
        public virtual BlockData Data
        {
            get { return blockData; }
        }

        private void Update()
        {
            if(blockData != _lastBlockData)
            {
                if (blockData != null && blockRendererStrategy != null)
                {
                    blockRendererStrategy.SetData(blockData);
                }
            }
        }


        public void AddRotation(Vector3 rotation)
        {
            blockRotationStrategy.AddPermanentRotation(rotation);
        }
    }

}
