using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable]
    public class BlockMetaData : ScriptableObject
    {
        public BlockType BlockType;
        public BlockId Id;
        public Sprite Sprite;
        public Material Material;

        public virtual BlockDataType BlockDataType => BlockDataType.None;
    }
}