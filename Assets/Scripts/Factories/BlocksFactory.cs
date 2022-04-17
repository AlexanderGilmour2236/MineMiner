using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MineMiner 
{
    public class BlocksFactory : MonoBehaviour
    {
        [SerializeField] private DestroyableBlockView _defaultDestroyableBlockPrefab;
        
        [SerializeField] private BlockView _droppedBlockPrefab;
        [SerializeField] private SpiteBlockView _droppedSpriteBlockPrefab;
        [SerializeField] private DestroyableBlockData _defaultBlockData;
        
        private float? _cachedBlockSize;

        public float BlockSize
        {
            get
            {
                if (_cachedBlockSize == null)
                {
                    _cachedBlockSize = _defaultDestroyableBlockPrefab.gameObject.GetComponent<BoxCollider>().bounds.size.x;
                }
                return _cachedBlockSize.Value;
            }
        }

        public BlockView[] GetDroppedBlockViews(DestroyableBlockData blockData)
        {
            int droppedBlocksCount = 1;
            DroppedBlockData droppedBlockData = blockData.DroppedBlockData;
            if (droppedBlockData != null)
            {
                droppedBlocksCount = Random.Range(droppedBlockData.DroppedMinCount, droppedBlockData.DroppedMaxCount);
            }
            
            BlockView[] droppedBlocks = new BlockView[droppedBlocksCount];
            for (int i = 0; i < droppedBlocksCount; i++)
            {
                BlockView blockView = Instantiate(GetDroppedBlockPrefab(blockData));
                blockView.SetData(blockData);
                droppedBlocks[i] = blockView;
            }
            
            return droppedBlocks;
           
        }

        private BlockView GetDroppedBlockPrefab(DestroyableBlockData blockData)
        {
            if (blockData.DroppedBlockData == null)
            {
                return _droppedBlockPrefab;
            }

            return GetDroppedBlockPrefab(blockData.DroppedBlockData);
        }

        private BlockView GetDroppedBlockPrefab(BlockData blockDataDroppedBlockData)
        {
            BlockType blockType = blockDataDroppedBlockData.BlockType;
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

        public DestroyableBlockView GetDestroyableBlockView(DestroyableBlockData blockData = null, Transform parent = null)
        {
            if (parent == null)
            {
                parent = transform;
            }

            if (blockData == null)
            {
                blockData = _defaultBlockData;
            }

            DestroyableBlockView blockView = Instantiate(GetDestroyableBlockPrefab(blockData), parent);
            blockView.SetData(blockData);
            return blockView;
        }

        private DestroyableBlockView GetDestroyableBlockPrefab(DestroyableBlockData blockData)
        {
            return _defaultDestroyableBlockPrefab;
        }
    }
}