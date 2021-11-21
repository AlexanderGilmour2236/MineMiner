using System.Collections.Generic;
using MineMiner;
using UnityEngine;

namespace DefaultNamespace
{
    public class BlockDataChanger : MonoBehaviour
    {
        private List<DestroyableBlockView> _currentBlocks = new List<DestroyableBlockView>();

        [ContextMenu("ChangeData")]
        public void SetDefaultData()
        {
            transform.GetComponentsInChildren(transform, _currentBlocks);
            foreach (DestroyableBlockView blockView in _currentBlocks)
            {
                BlockData blockData = blockView.Data;
                blockView.gameObject.AddComponent<DestroyableBlockView>().SetData(blockData);
                DestroyImmediate(blockView);
            }
        }
    }
}