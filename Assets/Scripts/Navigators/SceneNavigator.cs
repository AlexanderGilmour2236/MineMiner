using System;
using UnityEngine.SceneManagement;

namespace MineMiner
{
    public class SceneNavigator : Navigator
    {
        protected readonly string _sceneName;

        public SceneNavigator(String sceneName)
        {
            _sceneName = sceneName;
        }
        
        public override void Go()
        {
            base.Go();
            LoadScene(_sceneName);
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}