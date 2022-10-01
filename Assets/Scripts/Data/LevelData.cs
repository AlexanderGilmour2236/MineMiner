using System.Collections.Generic;
using DefaultNamespace;
using SimpleJSON;
using UnityEngine;

namespace MineMiner
{
    public class LevelData : IJSONObject
    {
        private List<BlockData> _blockDatas = new List<BlockData>();
        
        public LevelData(JSONNode jsonNode)
        {
            Debug.Log("nodeConstructor");
            BlockDataProvider blockDataProvider = new BlockDataProvider();
            
            JSONArray blockDataNodes = jsonNode[JSONKeys.LevelData].AsArray;
            foreach (JSONNode blockDataNode in blockDataNodes)
            {
                Debug.Log(blockDataNode[JSONKeys.BlockID].ToString());
                _blockDatas.Add(blockDataProvider.GetBlockData(blockDataNode));
            }
        }

        public LevelData(List<BlockData> blockDatas)
        {
            Debug.Log("Constructor");

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