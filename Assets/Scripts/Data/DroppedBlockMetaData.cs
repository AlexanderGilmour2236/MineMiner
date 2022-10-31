using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable][CreateAssetMenu(fileName = "newDroppedBlockData", menuName = "BlockData/Blocks/DroppedBlock")]
    public class DroppedBlockMetaData : BlockMetaData
    {
        public override BlockDataType BlockDataType => BlockDataType.Dropped;
    }
}