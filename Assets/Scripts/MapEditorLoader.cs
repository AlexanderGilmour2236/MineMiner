using System;
using UnityEngine;
using Zenject;

namespace MineMiner
{
    public class MapEditorLoader : MonoBehaviour
    {
        [Inject] private MapEditorNavigator _mapEditorNavigator;

        private void Start()
        {
            _mapEditorNavigator.Go();
        }
    }
}