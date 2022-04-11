using UnityEngine;

namespace MineMiner
{
    public class SpriteBlockRendererStrategy : BlockRendererStrategy
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public override void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
    }
}