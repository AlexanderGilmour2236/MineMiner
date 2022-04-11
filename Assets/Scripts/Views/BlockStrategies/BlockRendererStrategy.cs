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
            switch (blockData._blockType)
            {
                case BlockType.DefaultBlock:
                    SetMaterial(blockData._material);
                    break;
                case BlockType.SpriteBlock:
                    SetSprite(blockData._sprite);
                    break;
            }
        }
    }
}