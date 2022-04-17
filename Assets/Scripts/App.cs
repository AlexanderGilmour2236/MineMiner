using Zenject;

namespace MineMiner
{
    public class App : ITickable
    {
        private static App _instance;
        private Navigator _activeNavigator;

        public virtual void StartGame() { }

        public App()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public static App Instance()
        {
            if (_instance == null)
            {
                _instance = new App();
            }

            return _instance;
        }

        public void Tick()
        {
            _activeNavigator.Tick();
        }

        public void SetActiveNavigator(Navigator navigator)
        {
            _activeNavigator = navigator;
        }
    }
}