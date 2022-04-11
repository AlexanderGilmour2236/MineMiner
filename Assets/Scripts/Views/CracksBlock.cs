using UnityEngine;

namespace MineMiner
{
    public class CracksBlock : MonoBehaviour
    {
        [SerializeField] private Material[] _sortedMaterials;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        public void SetNormalizedValue(float value)
        {
            value = Mathf.Clamp(value, 0,1);
            int index = (int)(_sortedMaterials.Length * value);
            _meshRenderer.material = _sortedMaterials[index];
        }
    }
}