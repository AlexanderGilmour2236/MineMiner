using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace MineMiner 
{
    public class BlocksFactory : MonoBehaviour
    {
        [SerializeField] BlockView _defaultBlockPrefab;
        [SerializeField] BlockView _droppedBlockPrefab;
        [SerializeField] SpiteBlockView _droppedSpriteBlockPrefab;

        public BlockView[] GetDroppedBlockView(DestroyableBlockData blockData)
        {
            int randomValue = 1;
            DroppedBlockData droppedBlockData = blockData.DroppedBlockData;
            if (droppedBlockData != null)
            {
                randomValue = Random.Range(droppedBlockData.DroppedMinCount, droppedBlockData.DroppedMaxCount);
            }
            
            BlockView[] droppedBlocks = new BlockView[randomValue];
            for (int i = 0; i < randomValue; i++)
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
            
            switch (blockData.DroppedBlockData._blockType)
            {
                case BlockType.DefaultBlock:
                    return _droppedBlockPrefab;
                case BlockType.SpriteBlock:
                    return _droppedSpriteBlockPrefab;
            }

            return null;
        }
    }
}