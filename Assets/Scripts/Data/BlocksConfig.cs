using System.Collections.Generic;
using UnityEngine;

namespace MineMiner
{
    [CreateAssetMenu(fileName = "BlocksConfig", menuName = "Configs/BlocksConfig")]
    public class BlocksConfig : ScriptableObject
    {
        [SerializeField] private List<DestroyableBlockMetaData> _destroyableBlockMetaDatas;

        private Dictionary<BlockId, BlockMetaData> _blockIdToMetaData = new Dictionary<BlockId, BlockMetaData>();

        public BlockMetaData GetBlockMetaData(BlockId blockId)
        {
            if (_blockIdToMetaData.Count == 0)
            {
                GenerateBlocksDataDictionary();
            }
            return _blockIdToMetaData[blockId];
        }

        private void GenerateBlocksDataDictionary()
        {
            foreach (BlockMetaData blockMetaData in _destroyableBlockMetaDatas)
            {
                _blockIdToMetaData.Add(blockMetaData.Id, blockMetaData);
            }
        }

        [ContextMenu("Find All Blocks")]
        private void FindAllBLocks()
        {
            _destroyableBlockMetaDatas = FindAssetsOfType.FindAssetsByType<DestroyableBlockMetaData>();
        }

        public List<DestroyableBlockMetaData> DestroyableBlockMetaDatas
        {
            get { return _destroyableBlockMetaDatas; }
        }
    }
}