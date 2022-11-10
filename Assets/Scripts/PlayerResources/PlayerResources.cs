using System;
using System.Collections;
using System.Collections.Generic;
using MineMiner;
using UnityEngine;

namespace ResourcesProvider
{
    public class PlayerResources
    {
        private Dictionary<BlockId, PlayerResource> _resourcesDictionary = new Dictionary<BlockId, PlayerResource>();
        public event Action<PlayerResource> onResourceChange;

        public virtual PlayerResource GetPlayerResource(BlockId blockId)
        {
            PlayerResource playerResource = null;
            if (_resourcesDictionary.ContainsKey(blockId))
            {
                playerResource = _resourcesDictionary[blockId];
            }
            else
            {
                playerResource = new PlayerResource(blockId, 0);
                _resourcesDictionary[blockId] = playerResource;
            }

            return playerResource;
        }

        public PlayerResource AddResource(BlockId blockId, int incrementValue)
        {
            PlayerResource playerResource = GetPlayerResource(blockId);
            playerResource.Amount += incrementValue;
            onResourceChange?.Invoke(playerResource);
            return playerResource;
        }

        public PlayerResource SetResources(BlockId blockId, int value)
        {
            PlayerResource playerResource = GetPlayerResource(blockId);
            playerResource.Amount = value;
            return playerResource;
        }
    }

}
