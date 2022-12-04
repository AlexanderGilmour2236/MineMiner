using UnityEngine.SceneManagement;

namespace MineMiner
{
    public class App : ITickable
    {
        protected Navigator _activeNavigator;
        protected static App _instance;

        public App()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public virtual void StartGame(SceneType startScene)
        {
            GlobalUpdater.Instance.AddTickable(this);
        }

        public void Tick()
        {
            _activeNavigator.Tick();
        }
        
        public static App Instance()
        {
            if (_instance == null)
            {
                _instance = new MineMinerApp();
            }

            return _instance;
        }

        public void SetActiveNavigator(Navigator navigator)
        {
            _activeNavigator = navigator;
        }

        public static void RestartApp()
        {
            GlobalUpdater.Instance.ClearTickables();
            SceneManager.LoadScene(SceneNames.MAIN_LOADER_SCENE);
        }
    }
}