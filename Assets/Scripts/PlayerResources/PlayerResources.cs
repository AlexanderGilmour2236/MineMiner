using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourcesProvider
{
    public class PlayerResources
    {
        private Dictionary<int, PlayerResource> _resourcesDictionary = new Dictionary<int, PlayerResource>();

        public virtual PlayerResource GetPlayerResource(int resourceId)
        {
            PlayerResource playerResource = null;
            if (_resourcesDictionary.ContainsKey(resourceId))
            {
                playerResource = _resourcesDictionary[resourceId];
            }
            else
            {
                playerResource = new PlayerResource(resourceId, 0);
                _resourcesDictionary[resourceId] = playerResource;
            }

            return playerResource;
        }

        public PlayerResource AddResource(int resourceId, int incrementValue)
        {
            PlayerResource playerResource = GetPlayerResource(resourceId);
            playerResource.Amount += incrementValue;
            return playerResource;
        }

        public PlayerResource SetResources(int resourceId, int value)
        {
            PlayerResource playerResource = GetPlayerResource(resourceId);
            playerResource.Amount = value;
            return playerResource;
        }
    }

}
