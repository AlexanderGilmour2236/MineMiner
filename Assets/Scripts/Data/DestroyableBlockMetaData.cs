using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable][CreateAssetMenu(fileName = "newBlockData", menuName = "BlockData/Blocks/DestroyableBlock")]
    public class DestroyableBlockMetaData : BlockMetaData
    {
        public float Strength;
        public BlockMetaData droppedBlockMetaData;
        public int DroppedMinCount;
        public int DroppedMaxCount;
        
        public override BlockDataType BlockDataType => BlockDataType.Destroyable;
    }
}