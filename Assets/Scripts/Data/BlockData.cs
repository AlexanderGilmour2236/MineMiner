using System;
using DefaultNamespace;
using SimpleJSON;
using UnityEngine;

namespace MineMiner
{
    public class BlockData : IJSONObject
    {
        protected BlockMetaData _blockMetaData;
        protected BlockView _blockView;
        protected BlockId _blockId;
        
        public BlockData(BlockMetaData blockMetaData, BlockView blockView)
        {
            _blockMetaData = blockMetaData;
            _blockView = blockView;
        }

        public BlockData(JSONNode jsonNode)
        {
            _blockId = (BlockId)Enum.Parse(typeof(BlockId), jsonNode[JSONKeys.BlockID].Value);
        }

        public BlockMetaData BlockMetaData
        {
            get => _blockMetaData;
            set => _blockMetaData = value;
        }

        public BlockView BlockView => _blockView;

        public virtual JSONNode ToJson()
        {
            JSONNode blockDataNode = new JSONObject();
            blockDataNode.Add(JSONKeys.BlockID, _blockMetaData.Id.ToString());
            return blockDataNode;
        }
        
        public virtual BlockDataType BlockDataType => BlockDataType.None;
        public BlockId BlockId => _blockId;
    }
}