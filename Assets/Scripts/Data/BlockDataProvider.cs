using System;
using SimpleJSON;

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
                    return new DestroyableBlockData(jsonNode);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}