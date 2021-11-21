using System;
using UnityEngine;

namespace MineMiner
{
    [Serializable][CreateAssetMenu(fileName = "newBlockData", menuName = "BlockData")]
    public class BlockData : ScriptableObject
    {
        public Material Material;
        public float Strength;
    }
}