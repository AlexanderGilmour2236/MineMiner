using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable][CreateAssetMenu(fileName = "newDroppedBlockData", menuName = "BlockData/Blocks/DroppedBlock")]
    public class DroppedBlockData : BlockData
    {
        public int DroppedMinCount;
        public int DroppedMaxCount;
    }
}