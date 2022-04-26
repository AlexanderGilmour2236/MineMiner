using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable][CreateAssetMenu(fileName = "newBlockData", menuName = "BlockData/Blocks/DestroyableBlock")]
    public class DestroyableBlockMetaData : BlockMetaData
    {
        public float Strength;
        public DroppedBlockMetaData droppedBlockMetaData;
        
        public override BlockDataType BlockDataType => BlockDataType.Destroyable;
    }
}