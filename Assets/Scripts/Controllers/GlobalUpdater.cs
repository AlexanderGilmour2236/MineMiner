using System;
using System.Collections.Generic;
using UnityEngine;

namespace MineMiner
{
    public class GlobalUpdater : MonoBehaviour
    {
        private static GlobalUpdater _instance;

        public static GlobalUpdater Instance
        {
            get
            {
                if (_instance == null)
                {
                    SpawnUpdater();
                }
                return _instance;
            }
        }

        private List<ITickable> _tickableList = new List<ITickable>();

        private static void SpawnUpdater()
        {
            if (_instance != null)
            {
                return;
            }
            _instance = new GameObject().AddComponent<GlobalUpdater>();
            _instance.gameObject.name = nameof(GlobalUpdater);
            DontDestroyOnLoad(_instance);
        }

        public void AddTickable(ITickable tickable)
        {
            _tickableList.Add(tickable);
        }

        public void RemoveTickable(ITickable tickable)
        {
            
        }

        private void Update()
        {
            foreach (ITickable tickable in _tickableList)
            {
                if (tickable != null)
                {
                    tickable.Tick();
                }
            }
        }

        public void ClearTickables()
        {
            _tickableList.Clear();
        }
    }
}