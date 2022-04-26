using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MineMiner 
{
    public class BlocksFactory : MonoBehaviour
    {
        [SerializeField] private BlocksConfig _blocksConfig;
        [SerializeField] private DestroyableBlockView _defaultDestroyableBlockPrefab;
        
        [SerializeField] private BlockView _droppedBlockPrefab;
        [SerializeField] private SpiteBlockView _droppedSpriteBlockPrefab;
        [SerializeField] private DestroyableBlockMetaData defaultBlockMetaData;
        
        
        private float? _cachedBlockSize;

        public float BlockSize
        {
            get
            {
                if (_cachedBlockSize == null)
                {
                    BoxCollider boxCollider =  Instantiate(_defaultDestroyableBlockPrefab.gameObject.GetComponent<BoxCollider>());
                    _cachedBlockSize = boxCollider.bounds.size.x;
                    Destroy(boxCollider.gameObject);
                }
                return _cachedBlockSize.Value;
            }
        }

        public BlockView[] GetDroppedBlockViews(DestroyableBlockMetaData blockMetaData)
        {
            int droppedBlocksCount = 1;
            DroppedBlockMetaData droppedBlockMetaData = blockMetaData.droppedBlockMetaData;
            if (droppedBlockMetaData != null)
            {
                droppedBlocksCount = Random.Range(droppedBlockMetaData.DroppedMinCount, droppedBlockMetaData.DroppedMaxCount);
            }
            
            BlockView[] droppedBlocks = new BlockView[droppedBlocksCount];
            for (int i = 0; i < droppedBlocksCount; i++)
            {
                BlockView blockView = Instantiate(GetDroppedBlockPrefab(blockMetaData));
                blockView.SetMetaData(blockMetaData);
                droppedBlocks[i] = blockView;
            }
            
            return droppedBlocks;
           
        }

        private BlockView GetDroppedBlockPrefab(DestroyableBlockMetaData blockMetaData)
        {
            if (blockMetaData.droppedBlockMetaData == null)
            {
                return _droppedBlockPrefab;
            }

            return GetDroppedBlockPrefab(blockMetaData.droppedBlockMetaData);
        }

        private BlockView GetDroppedBlockPrefab(BlockMetaData blockMetaDataDroppedBlockMetaData)
        {
            BlockType blockType = blockMetaDataDroppedBlockMetaData.BlockType;
            switch (blockType)
            {
                case BlockType.DefaultBlock:
                    return _droppedBlockPrefab;
                case BlockType.SpriteBlock:
                    return _droppedSpriteBlockPrefab;
                default: 
                    throw new Exception($"Cannot find block prefab with type: {blockType}");
            }
        }

        public DestroyableBlockView GetDestroyableBlockView(Vector3Int position, DestroyableBlockMetaData blockMetaData = null, Transform parent = null)
        {
            if (parent == null)
            {
                parent = transform;
            }

            if (blockMetaData == null)
            {
                blockMetaData = defaultBlockMetaData;
            }

            DestroyableBlockView blockView = Instantiate(GetDestroyableBlockPrefab(blockMetaData), parent);
            blockView.SetMetaData(blockMetaData);
            blockView.SetData(new DestroyableBlockData(blockMetaData, blockView, position));
            return blockView;
        }

        private DestroyableBlockView GetDestroyableBlockPrefab(DestroyableBlockMetaData blockMetaData)
        {
            return _defaultDestroyableBlockPrefab;
        }
        
        public DestroyableBlockMetaData DefaultBlockMetaData => defaultBlockMetaData;

        public BlockMetaData GetBlockMetaData(BlockId blockId)
        {
            return _blocksConfig.GetBlockMetaData(blockId);
        }
    }
}