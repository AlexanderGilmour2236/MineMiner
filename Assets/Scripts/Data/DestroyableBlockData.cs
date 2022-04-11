using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable][CreateAssetMenu(fileName = "newBlockData", menuName = "BlockData/Blocks/DestroyableBlock")]
    public class DestroyableBlockData : BlockData
    {
        public float Strength;
        public DroppedBlockData DroppedBlockData;
    }
}