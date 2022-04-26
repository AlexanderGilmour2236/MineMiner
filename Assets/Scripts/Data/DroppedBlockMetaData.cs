using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable][CreateAssetMenu(fileName = "newDroppedBlockData", menuName = "BlockData/Blocks/DroppedBlock")]
    public class DroppedBlockMetaData : BlockMetaData
    {
        public int DroppedMinCount;
        public int DroppedMaxCount;
        
        public override BlockDataType BlockDataType => BlockDataType.Dropped;
    }
}