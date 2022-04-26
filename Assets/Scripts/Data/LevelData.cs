using System.Collections.Generic;
using DefaultNamespace;
using SimpleJSON;

namespace MineMiner
{
    public class LevelData : IJSONObject
    {
        private List<BlockData> _blockDatas = new List<BlockData>();
        
        public LevelData(JSONNode jsonNode)
        {
            BlockDataProvider blockDataProvider = new BlockDataProvider();

            foreach (JSONNode blockDataNode in jsonNode[JSONKeys.LevelData].AsArray)
            {
                _blockDatas.Add(blockDataProvider.GetBlockData(blockDataNode));
            }
        }

        public LevelData(List<BlockData> blockDatas)
        {
            _blockDatas = blockDatas;
        }

        public JSONNode ToJson()
        {
            JSONNode levelNode = new JSONObject();
            JSONArray jsonArray = new JSONArray();
            foreach (BlockData blockData in _blockDatas)
            {
                jsonArray.Add(blockData.ToJson());
            }

            levelNode.Add(JSONKeys.LevelData, jsonArray);
            return levelNode;
        }
        
        public List<BlockData> BlockDatas => _blockDatas;
    }
}