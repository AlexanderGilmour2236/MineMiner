using System;
using UnityEngine;
using Zenject;

namespace MineMiner 
{
    public class BlocksFactory : MonoBehaviour
    {
        [SerializeField] BlockView defaultBlockPrefab;
        [SerializeField] BlockView droppedBlockPrefab;
        [SerializeField] SpiteBlockView droppedSpriteBlockPrefab;

        public BlockView GetDroppedBlockView(DestroyableBlockData blockData)
        {
            BlockView blockView = Instantiate(GetDroppedBlockPrefab(blockData));
            blockView.SetData(blockData);
            return blockView;
           
        }

        private BlockView GetDroppedBlockPrefab(DestroyableBlockData blockData)
        {
            if (blockData.DroppedBlockData == null)
            {
                return droppedBlockPrefab;
            }
            
            switch (blockData.DroppedBlockData.BlockType)
            {
                case BlockType.DefaultBlock:
                    return droppedBlockPrefab;
                case BlockType.SpriteBlock:
                    return droppedSpriteBlockPrefab;
            }

            return null;
        }
    }
}