using UnityEngine;

namespace MineMiner
{
    public class MeshBlockRendererStrategy : BlockRendererStrategy
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        
        public override void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }
    }
}