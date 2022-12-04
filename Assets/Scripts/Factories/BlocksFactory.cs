using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MineMiner 
{
    public class BlocksFactory : MonoBehaviour
    {
        [SerializeField] private BlocksConfig _blocksConfig;
        [SerializeField] private DestroyableBlockView _defaultDestroyableBlockPrefab;
        
        [SerializeField] private DroppedBlockView _droppedBlockPrefab;
        [SerializeField] private SpiteBlockView _droppedSpriteBlockPrefab;
        [SerializeField] private DestroyableBlockMetaData defaultBlockMetaData;
        
        private MonoPool<DroppedBlockView> _droppedBlocksPool;
        private MonoPool<DroppedBlockView> _droppedSpriteBlocksPool;
        private float? _cachedBlockSize;

        private void Awake()
        {
            _droppedBlocksPool = new MonoPool<DroppedBlockView>(null, _droppedBlockPrefab);
            _droppedSpriteBlocksPool = new MonoPool<DroppedBlockView>(null, _droppedSpriteBlockPrefab);
        }

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

        public DroppedBlockView[] GetDroppedBlockViews(DestroyableBlockMetaData blockMetaData)
        {
            int droppedBlocksCount = 1;
            droppedBlocksCount = Random.Range(blockMetaData.DroppedMinCount, blockMetaData.DroppedMaxCount);
            print(droppedBlocksCount);
            DroppedBlockView[] droppedBlocks = new DroppedBlockView[droppedBlocksCount];
            for (int i = 0; i < droppedBlocksCount; i++)
            {
                DroppedBlockView blockView = GetDroppedBlockPool(blockMetaData.BlockType).GetObject();
                blockView.gameObject.name = $"gnida{i}";
                blockView.SetMetaData(blockMetaData);
                droppedBlocks[i] = blockView;
            }
            
            return droppedBlocks;
           
        }

        public MonoPool<DroppedBlockView> GetDroppedBlockPool(BlockType blockType)
        {
            if (blockType == BlockType.None)
            {
                return _droppedBlocksPool;
            }
            
            switch (blockType)
            {
                case BlockType.DefaultBlock:
                    return _droppedBlocksPool;
                case BlockType.SpriteBlock:
                    return _droppedSpriteBlocksPool;
                default: 
                    throw new Exception($"Cannot find block prefab with type: {blockType}");
            }
        }
        
        public MonoPool<DroppedBlockView> GetDroppedBlockPool(DroppedBlockView droppedBlockView)
        {
            if (_droppedBlocksPool.ContainsItem(droppedBlockView))
            {
                return _droppedBlocksPool;
            }
            if (_droppedSpriteBlocksPool.ContainsItem(droppedBlockView))
            {
                return _droppedSpriteBlocksPool;
            }

            return null;
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

        public DestroyableBlockMetaData[] GetAllBlocksData()
        {
            return _blocksConfig.DestroyableBlockMetaDatas.ToArray();
        }
    }
}