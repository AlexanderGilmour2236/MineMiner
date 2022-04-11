using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable]
    public class BlockData : ScriptableObject
    {
        public BlockType _blockType;
        public BlockId _id;
        public Sprite _sprite;
        public Material _material;

    }
}