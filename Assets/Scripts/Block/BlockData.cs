using System;
using UnityEngine;

namespace Game.Block
{
    [Serializable][CreateAssetMenu(fileName = "newBlockData", menuName = "BlockData")]
    public class BlockData : ScriptableObject
    {
        public Material Material;
        public float Strength;
    }
}