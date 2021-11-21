using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable][CreateAssetMenu(fileName = "newBlockData", menuName = "BlockData/Blocks/DefaultBlock")]
    public class DestroyableBlockData : BlockData
    {
        public float Strength;
        public BlockData DroppedBlockData;
    }
}