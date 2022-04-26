using UnityEngine;

namespace MineMiner
{
    public class BlockRendererStrategy : MonoBehaviour
    {
        public virtual void SetMaterial(Material material)
        {
        }

        public virtual void SetSprite(Sprite sprite)
        {
        }

        public void SetData(BlockMetaData blockMetaData)
        {
            switch (blockMetaData.BlockType)
            {
                case BlockType.DefaultBlock:
                    SetMaterial(blockMetaData.Material);
                    break;
                case BlockType.SpriteBlock:
                    SetSprite(blockMetaData.Sprite);
                    break;
            }
        }
    }
}