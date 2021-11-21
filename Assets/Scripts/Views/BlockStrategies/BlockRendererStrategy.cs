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

        public void SetData(BlockData blockData)
        {
            switch (blockData.BlockType)
            {
                case BlockType.DefaultBlock:
                    SetMaterial(blockData.Material);
                    break;
                case BlockType.SpriteBlock:
                    SetSprite(blockData.Sprite);
                    break;
            }
        }
    }
}