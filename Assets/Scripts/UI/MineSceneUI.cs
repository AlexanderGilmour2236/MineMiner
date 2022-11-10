using System.Collections.Generic;
using ResourcesProvider;
using UnityEngine;

namespace MineMiner
{
    public class MineSceneUI : MonoBehaviour
    {
        [SerializeField] private ResourceIndicator _goldsResourceIndicator;

        private Dictionary<BlockId, ResourceIndicator> _blockIdToResourceIndicator =
            new Dictionary<BlockId, ResourceIndicator>();

        public void Init(BlocksFactory blocksFactory)
        {
            _blockIdToResourceIndicator[BlockId.Golds] = _goldsResourceIndicator;
            _goldsResourceIndicator.SetSprite(blocksFactory.GetBlockMetaData(BlockId.Golds).Sprite);
            _goldsResourceIndicator.SetValueText("0");
        }

        public void UpdatePlayerResource(PlayerResource playerResource)
        {
            if (_blockIdToResourceIndicator.ContainsKey(playerResource.BlockId))
            {
                _blockIdToResourceIndicator[playerResource.BlockId].SetValueText(playerResource.Amount.ToString());
            }
        }
    }
}