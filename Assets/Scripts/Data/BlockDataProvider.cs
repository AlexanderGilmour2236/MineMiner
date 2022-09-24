using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace MineMiner
{
    public class BlockDataProvider
    {
        public BlockData GetBlockData(JSONNode jsonNode)
        {
            BlockDataType blockDataType = 
                (BlockDataType)Enum.Parse(typeof(BlockDataType),jsonNode[JSONKeys.BlockDataType].Value);
            
            switch (blockDataType)
            {
                case BlockDataType.None:
                    return new BlockData(jsonNode);
                case BlockDataType.Destroyable:
                    Debug.Log(jsonNode[JSONKeys.BlockID].ToString());
                    return new DestroyableBlockData(jsonNode, jsonNode[JSONKeys.BlockID].Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}