using UnityEngine;

namespace MineMiner
{
    public class SpriteBlockRendererStrategy : BlockRendererStrategy
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public override void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}