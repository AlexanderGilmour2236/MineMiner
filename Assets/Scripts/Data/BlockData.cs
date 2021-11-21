using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable]
    public class BlockData : ScriptableObject
    {
        public BlockType BlockType;
        public BlockId Id;
        public Sprite Sprite;
        public Material Material;

    }
}