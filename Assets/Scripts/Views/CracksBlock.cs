using UnityEngine;

namespace MineMiner
{
    public class CracksBlock : MonoBehaviour
    {
        [SerializeField] private Material[] sortedMaterials;
        [SerializeField] private MeshRenderer meshRenderer;
        
        public void SetNormalizedValue(float value)
        {
            value = Mathf.Clamp(value, 0,1);
            int index = (int)(sortedMaterials.Length * value);
            meshRenderer.material = sortedMaterials[index];
        }
    }
}